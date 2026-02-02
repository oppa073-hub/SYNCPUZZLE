using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance { get; private set; }
    [SerializeField] private AudioClip clikcSfx;
    [SerializeField] private AudioClip bgmSfx;

    [Header("Lobby Preview")]
    [SerializeField] private LobbyAvatarView avatarPrefab;
    [SerializeField] private Transform[] avatarSlots; 
    private readonly Dictionary<int, LobbyAvatarView> spawned = new Dictionary<int, LobbyAvatarView>();
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

    private void RefreshLobbyAvatars()
    {
        // 슬롯 비우기 다 지우고 다시 그림
        foreach (var kv in spawned) if (kv.Value) Destroy(kv.Value.gameObject);
        spawned.Clear();

        var players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length && i < avatarSlots.Length; i++)
        {
            var p = players[i];

            var view = Instantiate(avatarPrefab, avatarSlots[i].position, Quaternion.identity);
            spawned[p.ActorNumber] = view;

            var role = (p.IsMasterClient) ? PlayerRole.A : PlayerRole.B;
            view.Set(p.NickName, role);
        }
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
        RefreshLobbyAvatars();
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
        RefreshLobbyAvatars();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + "님이 나감");
        RefreshLobbyAvatars();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        roomBtn.interactable = PhotonNetwork.IsMasterClient;
        Debug.Log(newMasterClient.NickName + "님이 방장이 됨");
        RefreshLobbyAvatars();
    }
}
