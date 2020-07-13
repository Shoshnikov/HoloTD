using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Networking : MonoBehaviourPunCallbacks
{
    public bool isGameStart = false; //Если два игрока в лобби, то игра начинаетьсья
    private GameObject resp1;
    private GameObject resp2;

    void Start()
    {
        resp1 = GameObject.Find("Respawn");
        resp2 = GameObject.Find("Respawn1");
        PhotonNetwork.GameVersion = "1.0";  //Версия игры, что бы разные игры не коннектились
        PhotonNetwork.ConnectUsingSettings();    //Передаёт информацию на Фотон облако и пытаесть коннектиться к мастер клиенту
    }

    public override void OnConnectedToMaster() //После подключения к облаку пытаеться подлкючиться войти в руму
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Вы вошли в комнату");
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions { MaxPlayers = 2 });
        Debug.Log("Создание комнаты");
    }

    //public void JoinRoom() //Вход в комнату, думаю это ненужна функция
    //{
    //    PhotonNetwork.JoinRandomRoom();
    //}

    public override void OnJoinRandomFailed(short returnCode, string message) //Если нет комнаты в которую можно войти, создаём свою
    {
        CreateRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)  //Если не получилось создать комнату, то корабль точно тонет.
    {
        Debug.Log($"Создание комнаты не возможно. {returnCode}, {message}");
    }

    public override void OnLeftRoom() //Когды вышел из комнаты. Наверно надо ливнуть в лобби (которого нет ибо мртк странно работает)
    {
        Debug.Log("Выход из комнаты");
    }

    private void Update()
    {
        try
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount > 1 && !isGameStart)
            {
                //PhotonNetwork.Instantiate("Deer_tower_Model", new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0)); работает
                isGameStart = true;
                resp1.GetComponent<Spawn>().flag = isGameStart;
                resp2.GetComponent<Spawn>().flag = isGameStart;
                Debug.Log("GameStart");
            }
        }
        catch
        {
            Debug.Log("Вы ещё не в сети");
        }
    }
}
