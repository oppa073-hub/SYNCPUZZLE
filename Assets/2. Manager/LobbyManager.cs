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

    [SerializeField] Button createBtn;
    [SerializeField] Button joinBtn;
    [SerializeField] Button randomBtn;
    [SerializeField] Button exitBtn; 
    
    [SerializeField] private AudioClip bgmSfx;
    [SerializeField] private AudioClip clikcSfx;

    private void Start()
    {
        AudioManager.instance.PlayBGM(bgmSfx);
        roomHas = new Dictionary<string, GameObject>();
        createBtn.interactable = false;
        joinBtn.interactable = false;
        randomBtn.interactable = false;

       if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
        else if (!PhotonNetwork.InLobby)
            PhotonNetwork.JoinLobby();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("로비 들어옴");

        createBtn.interactable = true;
        joinBtn.interactable = true;
        randomBtn.interactable = true;
    }
   
    public void CreateRoom()
    {
        AudioManager.instance.PlaySFX(clikcSfx);
        Debug.Log($"State={PhotonNetwork.NetworkClientState} InLobby={PhotonNetwork.InLobby} InRoom={PhotonNetwork.InRoom} IsConnected={PhotonNetwork.IsConnected}");

        PhotonNetwork.CreateRoom(createRoomInput.text, new RoomOptions { MaxPlayers = 2 });  //인풋필드에 들어있던 내용의 이름으로 방 생성
    }
    public void JoinRandomRoom()
    {
        AudioManager.instance.PlaySFX(clikcSfx);
        Debug.Log($"State={PhotonNetwork.NetworkClientState} InLobby={PhotonNetwork.InLobby} InRoom={PhotonNetwork.InRoom} IsConnected={PhotonNetwork.IsConnected}");

        if (!PhotonNetwork.InLobby)
        {
            Debug.Log("아직 로비 접속중");
            return;
        }
        PhotonNetwork.JoinRandomOrCreateRoom();
    }
    public override void OnLeftLobby()
    {
        createBtn.interactable = false;
        joinBtn.interactable = false;
        randomBtn.interactable = false;
    }
    public void JoinRoom()
    {
        AudioManager.instance.PlaySFX(clikcSfx);
        Debug.Log($"State={PhotonNetwork.NetworkClientState} InLobby={PhotonNetwork.InLobby} InRoom={PhotonNetwork.InRoom} IsConnected={PhotonNetwork.IsConnected}");

        if (PhotonNetwork.InLobby == true)
        {
            PhotonNetwork.JoinRoom(joinRoomInput.text);
        }
        else
        {
            Debug.Log("아직 로비 접속중");
            return;
        }
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Room");
    }
    public void ExitLobby()
    {
        AudioManager.instance.PlaySFX(clikcSfx);
        PhotonNetwork.LeaveLobby();  //로비 떠나라고 포톤에게 지시
        SceneManager.LoadScene("Title");
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
