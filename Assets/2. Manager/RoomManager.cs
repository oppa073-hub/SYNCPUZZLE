using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
       
       StartCoroutine(StartRoutine());
       
    }
  
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(1);
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    } 
    //방장이면 게임 시작버튼 활성화
    //일반 참여자면 비활성화
    [SerializeField] Button roomBtn;

    private IEnumerator StartRoutine()
    {
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        Player[] players = PhotonNetwork.PlayerList;  //방 속 사람을 받아옴

        foreach (var p in players)
        {
            Debug.Log("방 안의 사람들 목록:" + p.NickName);
        }

        if (PhotonNetwork.IsMasterClient == false)
        {
            roomBtn.interactable = false;  //혹은 방장이 아니라면 text를 start 대신 Ready등등
        }
    }

    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient == true)
        {
            PhotonNetwork.LoadLevel("InGameScene");  //네트워크상에서 씬 바꾸는 것
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "님이 방에 입장함");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + "님이 나감");
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        roomBtn.interactable = PhotonNetwork.IsMasterClient;
        Debug.Log(newMasterClient.NickName + "님이 방장이 됨");
    }
}
