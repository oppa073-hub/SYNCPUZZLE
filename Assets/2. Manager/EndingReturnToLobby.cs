using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingReturnToLobby : MonoBehaviourPunCallbacks
{
    [SerializeField] private float waitSeconds = 3f;
    [SerializeField] private string lobbySceneName = "Lobby";

    private bool isReturning;

    private void Start()
    {
        StartCoroutine(ReturnRoutine());
    }

    private IEnumerator ReturnRoutine()
    {
        yield return new WaitForSeconds(waitSeconds);
        ReturnToLobby();
    }

    private void ReturnToLobby()
    {
        if (isReturning) return;
        isReturning = true;

        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            SceneManager.LoadScene(lobbySceneName);
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(lobbySceneName);
    }
}
