using UnityEngine;
using Photon.Pun;

public class SyncTransform : MonoBehaviour //Реализация синхронизации позиции движимых предметов, тк через фотоновые функции мне не давали двигать башню на клиенте 
{
    private PhotonView PV; //Его всё равно стоит добавлять на обьект, без него не заработает.
    private Rigidbody rb; //rigidbody который лежит на передвигаемом обтекте.

    [SerializeField] private bool SyncPosition = false;
    private Vector3 LastPos; //Последняя позиция

    [SerializeField] private bool SyncRotation = false;
    private Quaternion LastRot; //Последний разворот ну или как его там)) 

    [SerializeField] bool SyncScale = false;
    private Vector3 LastScale; //Последний размер

    [SerializeField] private bool SyncAnimAndKinem = false;
    private bool lastKinem = false;
    private bool lastAnim = true;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        LastPos = gameObject.transform.position;
        LastRot = gameObject.transform.rotation;
        LastScale = gameObject.transform.localScale;
    }

    private void Update()
    {
        if (LastPos != gameObject.transform.position && rb.isKinematic && SyncPosition) //Если с момента последнего update изменился position, и мы синхронизируем его, то отрпавлять. (Почему я не сравнил вектора хз) 
        {
            PV.RPC("RpcSendTransform", RpcTarget.All, gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }

        if (LastRot != gameObject.transform.rotation && rb.isKinematic && SyncRotation) //RB.isKinematic становиться true если мы берём предмет в руки 
        {
            PV.RPC("RpcSendRotation", RpcTarget.All, gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z, gameObject.transform.rotation.w);
        }

        if (LastScale != gameObject.transform.localScale && rb.isKinematic && SyncScale)
        {
            PV.RPC("RpcSendTransform", RpcTarget.All, gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        }
        //Надо останавливать предмет на месте, что мы он не улетал, когда его отпускаешь 
        //При присоединени нужно отправлять координату (Если можно подключаться во время игры)
        if (SyncAnimAndKinem && lastAnim != gameObject.GetComponent<Animator>().enabled && lastKinem != gameObject.GetComponent<Rigidbody>().isKinematic)
        {
            PV.RPC("RpcSendAnimatorAndKinematic", RpcTarget.All, gameObject.GetComponent<Animator>().enabled, gameObject.GetComponent<Rigidbody>().isKinematic);
        }
    }

    [PunRPC]
    private void RpcSendTransform(float x, float y, float z)
    {
        Vector3 takenPos = new Vector3(x, y, z);
        gameObject.transform.position = takenPos;
        LastPos = takenPos; //В прошлой реазлизации забыл изменить ластпозишн
        Debug.Log("RpcSendTransform");
    }

    [PunRPC]
    private void RpcSendRotation(float x, float y, float z, float w)
    {
        Quaternion takenRot = new Quaternion(x, y, z, w);
        gameObject.transform.rotation = takenRot;
        LastRot = takenRot;
        Debug.Log("RpcSendRotation");
    }

    [PunRPC]
    private void RpcSendScale(float x, float y, float z)  //В разработке, пока не работает
    {
        Vector3 takenScale = new Vector3(x, y, z);
        gameObject.transform.localScale = takenScale;
        Debug.Log("RpcSendScale");
    }

    [PunRPC]
    private void RpcSendAnimatorAndKinematic(bool Anim, bool kinematic)
    {
        gameObject.GetComponent<Animator>().enabled = Anim;
        gameObject.GetComponent<Rigidbody>().isKinematic = kinematic;
        lastAnim = Anim;
        lastKinem = kinematic;
        Debug.Log("RpcSendAnimatorAndKinematic");
    }

    [PunRPC]
    private void Message(string message)
    {
        Debug.Log($"Message {message}");
    }

    public void Mes(string message)
    {
        PV.RPC("Message", RpcTarget.All, message);
    }

}