using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_InputField createRoomInput;
    [SerializeField] TMP_InputField joinRoomInput;

    [SerializeField] GameObject roomButtonPrefab;
    [SerializeField] Transform roomListPanel;
    Dictionary<string, GameObject> roomHas;
  
    private void Start()
    {
        roomHas = new Dictionary<string, GameObject>();
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 들어옴");
    }
   
    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createRoomInput.text, new RoomOptions { MaxPlayers = 2 });  //인풋필드에 들어있던 내용의 이름으로 방 생성
    }
    public void JoinRandomRoom()
    {
        //PhotonNetwork.JoinRandomRoom();
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("RoomScene");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject roomButtonGO;
        foreach (RoomInfo roomInfo in roomList)
        {
            if (roomInfo.RemovedFromList)
            {
                if (roomHas.TryGetValue(roomInfo.Name, out roomButtonGO))
                {
                    Destroy(roomButtonGO);
                    roomHas.Remove(roomInfo.Name);
                }
                    continue;
            }
           
            if (roomHas.TryGetValue(roomInfo.Name, out roomButtonGO))
            {
                roomButtonGO.GetComponent<RoomEntryUI>().SetRoom(roomInfo.Name);
            }
            else
            {
                var room = Instantiate(roomButtonPrefab, roomListPanel);
                room.GetComponent<RoomEntryUI>().SetRoom(roomInfo.Name);
                roomHas[roomInfo.Name] = room;
                room.GetComponentInChildren<TMP_Text>().text = roomInfo.Name;
            }
        

        }
    }

}
