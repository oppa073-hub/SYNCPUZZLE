using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSessionManager : MonoBehaviourPunCallbacks
{
    public static GameSessionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log($"[GameSessionManager] {otherPlayer.NickName} left.");

        if (UIManager.Instance != null)
            UIManager.Instance.ShowLeaveAndReturn(otherPlayer.NickName);
    }

    //LeaveRoom 완료되면 여기로 들어옴
    public override void OnLeftRoom()
    {
        Debug.Log("[GameSessionManager] OnLeftRoom -> Load Lobby");

        if (UIManager.Instance != null)
            UIManager.Instance.HideLeaveUI();

        SceneManager.LoadScene("Lobby");
    }

    //네트워크가 끊겨도 로비로 보내기
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarning($"[GameSessionManager] Disconnected: {cause}");

        if (UIManager.Instance != null)
            UIManager.Instance.HideLeaveUI();

        SceneManager.LoadScene("Lobby");
    }
}
