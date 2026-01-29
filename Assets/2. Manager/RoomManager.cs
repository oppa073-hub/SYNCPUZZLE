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
    [SerializeField] private AudioClip clikcSfx;
    [SerializeField] private AudioClip bgmSfx;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        AudioManager.instance.PlayBGM(bgmSfx);
        StartCoroutine(StartRoutine());
       
    }
  
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
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
        if (!PhotonNetwork.IsMasterClient) return;
        AudioManager.instance.PlaySFX(clikcSfx);
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("Stage1");
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
