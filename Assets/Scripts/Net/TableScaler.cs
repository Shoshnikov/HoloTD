using Microsoft.MixedReality.Toolkit;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class TableScaler : MonoBehaviour
{
    [SerializeField] [Range(1, 90)] private byte rotationAngle = 1;
    [SerializeField] [Range(0.001f, 0.1f)] private float scaleIndex = 0.1f;
    [SerializeField] [Range(0, 5)] private float differenceIndex = 0.2f;
    [SerializeField] private Transform[] Ray;
    private Transform transform;

    private Vector3 down = new Vector3(0, -1f, 0);

    private void Awake()
    {
        transform = gameObject.GetComponent<Transform>();
    }

    private void Start()
    {
        Invoke("DropTable", 15);
    }

    public void DropTable()
    {
        Debug.Log("Table teleport");
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
    }

    private void Update()
    {
        TestRays();
    }

    private void UpdRays(ref Ray[] rays)
    {
        for (int i = 0; i < Ray.Length; ++i)
        {
            rays[i] = new Ray(Ray[i].position, down);
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
            rays[i] = new Ray(Ray[i].position, down);
            Physics.Raycast(rays[i], out hits[i]);
            Debug.DrawRay(Ray[i].position, rays[i].direction * hits[i].distance, Color.red);
        }
    }
}