using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR.WSA.Sharing;
using Microsoft.MixedReality.Toolkit.Experimental.Utilities;
using UnityEngine.XR.WSA.Input;
using UnityEngine.XR.WSA;
using UnityEngine.XR.WSA.Persistence;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PhotonView))]

public class TableScaler : MonoBehaviourPunCallbacks
{
    [SerializeField] [Range(1, 90)] private byte rotationAngle = 1;
    [SerializeField] [Range(0.001f, 0.1f)] private float scaleIndex = 0.1f;
    [SerializeField] [Range(0, 5)] private float differenceIndex = 0.2f;
    [SerializeField] private Transform[] Ray;
    private Transform transform;
    private PhotonView photonView;
    private WorldAnchorManager worldAnchorManager = new WorldAnchorManager();
    private WorldAnchorStore worldAnchorStore;
    private bool savedAnchor;
    private int retryCount = 10;
    public byte[] fileData { get; private set; }

    private void Awake()
    {
        transform = gameObject.GetComponent<Transform>();
        photonView = gameObject.GetComponent<PhotonView>();
    }

    public void Table(int w8Time)
    {
        Invoke("DropTable", w8Time);
    }

    private void DropTable()
    {
        transform.position = CoreServices.InputSystem.GazeProvider.HitInfo.point;  //Отрпавляет стол на положение гейза.
        transform.Translate(Vector3.up * 0.03f);
        ScaleTable();
    }

    private void ScaleTable()
    {
        //Тут должно быть нахождения центра стола, пока прост на гейз кидает.

        RaycastHit[] hits = new RaycastHit[Ray.Length]; //Хранит всё о попадании.
        Ray[] rays = new Ray[Ray.Length]; //Хранит все лучи.
        Vector3 MaxScale = transform.localScale;
        Quaternion onAngleMaxScale = transform.rotation;
        Vector3 scaleChange = new Vector3(scaleIndex, scaleIndex, scaleIndex);

        UpdRays(ref rays);

        for (int rotarion = 0; rotarion < 90; rotarion += rotationAngle) //Достаточно только 90 градусов поворота для нахождения самого большого значения скейла
        {
            while (CanChageSize(ref rays, ref hits))
            {
                if (transform.localScale.x > MaxScale.x)    //По всем осям скейл одинаковый, можно сделать и так.
                {
                    MaxScale = transform.localScale;
                    onAngleMaxScale = transform.rotation;
                }
                transform.localScale += new Vector3(scaleIndex, scaleIndex, scaleIndex);
            }

            transform.Rotate(0, -rotationAngle, 0);
        }

        transform.rotation = onAngleMaxScale;
        transform.localScale = MaxScale;
        ExportWorldAnchor();
    }

    private void Update()
    {
        //TestRays();
    }

    private void UpdRays(ref Ray[] rays)
    {
        for (int i = 0; i < Ray.Length; ++i)
        {
            rays[i] = new Ray(Ray[i].position, Vector3.down);
        }
    }

    private bool CanChageSize(ref Ray[] rays, ref RaycastHit[] hits)    //Проверяет достигли ли все лучи цели и разницу между длинами
    {
        UpdRays(ref rays);
        bool res = difference(hits, differenceIndex);
        for (int i = 0; i < Ray.Length; ++i)
        {
            res &= Physics.Raycast(rays[i], out hits[i]);
        }
        return res;
    }

    private bool difference(RaycastHit[] arr, float diff) //Возвращает если максимальная разница величин меньше либо равна diff
    {
        float max = arr[0].distance;
        float min = arr[0].distance;

        for (int i = 0; i < arr.Length; ++i)
        {
            if (arr[i].distance > max) { max = arr[i].distance; }
            if (arr[i].distance < min) { min = arr[i].distance; }
            if (arr[i].distance == 0) { return false; }
        }
        return ((max - min > diff) ? false : true);
    }

    private void TestRays() 
    {
        RaycastHit[] hits = new RaycastHit[Ray.Length];
        Ray[] rays = new Ray[Ray.Length];

        for (int i = 0; i < Ray.Length; ++i)
        {
            rays[i] = new Ray(Ray[i].position, Vector3.down);
            Physics.Raycast(rays[i], out hits[i]);
            Debug.DrawRay(Ray[i].position, rays[i].direction * hits[i].distance, Color.red);
        }
    }

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Table(7);
        }
    }

    public override void OnLeftRoom()
    {
        worldAnchorManager.RemoveAnchor(this.gameObject);
    }

    [PunRPC] private void SendAnchor(byte[] data)
    {
        Debug.Log($"Якорь принят {data}");
        fileData = data;
        WorldAnchorTransferBatch.ImportAsync(data, OnImportComplete);
    }

    private void OnImportComplete(SerializationCompletionReason completionReason, WorldAnchorTransferBatch deserializedTransferBatch)
    {
        if (completionReason != SerializationCompletionReason.Succeeded)
        {
            Debug.Log("Failed to import: " + completionReason.ToString());
            if (retryCount > 0)
            {
                retryCount--;
                WorldAnchorTransferBatch.ImportAsync(fileData, OnImportComplete);
            }
            return;
        }
        deserializedTransferBatch.LockObject("TableAnchor", gameObject);
    }


    private void ExportWorldAnchor()   //Wrong
    {
        worldAnchorManager.AttachAnchor(this.gameObject, "TableAnchor");
        WorldAnchor MyWorlAnchor = worldAnchorManager.AnchorStore.Load("TableAnchor", this.gameObject);
        WorldAnchorTransferBatch transferBatch = new WorldAnchorTransferBatch();
        transferBatch.AddWorldAnchor("GameRootAnchor", MyWorlAnchor);
        WorldAnchorTransferBatch.ExportAsync(transferBatch, OnExportDataAvailable, OnExportComplete);
    }

    private void OnExportComplete(SerializationCompletionReason completionReason)
    {
        if (completionReason == SerializationCompletionReason.Succeeded)
        {
            Debug.Log("Якорь был сериализован");
        }
        else
        {
            Debug.Log("Якорь не был сериализован");
        }
    }

    private void OnExportDataAvailable(byte[] data)
    {
        photonView.RPC("SendAnchor", RpcTarget.OthersBuffered, data);
    }
}