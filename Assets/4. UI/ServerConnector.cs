using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class ServerConnector : MonoBehaviourPunCallbacks
{
    [SerializeField] private AudioClip bgmSfx;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

    }
    private void Start()
    {
        AudioManager.instance.PlayBGM(bgmSfx);
    }
    public void ConnectToServer()  //버튼 연결 메서드
    {
        if (PhotonNetwork.IsConnected) { return; }
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "kr";
        PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = true;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 연결");
        //AudioManager.instance.StopBGM()
        if (PhotonNetwork.IsConnected) { return; }
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "kr";
        PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = true;
        PhotonNetwork.ConnectUsingSettings();
    }

}
