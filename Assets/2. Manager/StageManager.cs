using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviourPunCallbacks
{
    public static StageManager instance { get; private set; }
    string[] stageSceneNames;
    int currentStageIndex = 0;
    // key = ActorNumber, value = inGoal
    private readonly Dictionary<int, bool> playersInGoal = new Dictionary<int, bool>();

    private bool isLoading;

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);

        stageSceneNames = new string[4];
        stageSceneNames[1] = "Stage2";
        stageSceneNames[2] = "Stage3";

    }

    public void ReportGoalState(bool inGoal)
    {
        if (!PhotonNetwork.InRoom) return;

        int actor = PhotonNetwork.LocalPlayer.ActorNumber;

        // 마스터에게만 보고
        photonView.RPC(nameof(RPC_ReportGoalStateToMaster), RpcTarget.MasterClient, actor, inGoal);
    }

    [PunRPC]
    private void RPC_ReportGoalStateToMaster(int actor, bool inGoal, PhotonMessageInfo info)
    {

        if (!PhotonNetwork.IsMasterClient) return;

        // 기록
        playersInGoal[actor] = inGoal;
        BroadcastGoalUI();
        // 판정
        CheckAllPlayersInGoal_AndLoad();
    }
    [PunRPC]
    private void RPC_UpdateGoalUI(int count, int total)
    {
        if (UIManager.Instance != null)
            UIManager.Instance.UpdateGoalUI(count, total);
    }
    private void BroadcastGoalUI()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (PhotonNetwork.CurrentRoom == null) return;

        int total = PhotonNetwork.CurrentRoom.PlayerCount;

        int count = 0;
        foreach (Player p in PhotonNetwork.PlayerList)
        {
            if (playersInGoal.TryGetValue(p.ActorNumber, out bool inGoal) && inGoal)
                count++;
        }

        photonView.RPC(nameof(RPC_UpdateGoalUI), RpcTarget.All, count, total);
    }

    private void CheckAllPlayersInGoal_AndLoad()
    {

        if (isLoading) return;
        if (PhotonNetwork.CurrentRoom == null) return;

        int roomCount = PhotonNetwork.CurrentRoom.PlayerCount;

        // 방 인원수만큼 모두 true 인지 확인
        // 1 playersInGoal에 방 인원이 전부 들어와있는지
        // 2 그 전부가 true인지
        if (playersInGoal.Count < roomCount) return;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            int actor = p.ActorNumber;

            if (!playersInGoal.TryGetValue(actor, out bool inGoal) || !inGoal)
            {
                return; // 한명이라도 없거나 false면 실패
            }
        }

        // 여기까지 오면 전원 도착
        isLoading = true;
        BroadcastGoalUI();
        int nextIndex = currentStageIndex + 1;

        PhotonNetwork.LoadLevel(stageSceneNames[nextIndex]);
        currentStageIndex = nextIndex;
        StartCoroutine(ResetLoadingNextFrame());
    }
    private IEnumerator ResetLoadingNextFrame()
    {
        // 최소 1~2프레임 기다리기
        yield return null;
        yield return null;

        isLoading = false;
        playersInGoal.Clear();
    }
    public override void OnLeftRoom()
    {
        playersInGoal.Clear();
        isLoading = false;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        // 누가 나가면 상태 업데이트 후 다시 판정
        if (!PhotonNetwork.IsMasterClient) return;

        playersInGoal.Remove(otherPlayer.ActorNumber);
        BroadcastGoalUI();
        CheckAllPlayersInGoal_AndLoad();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        // 누가 들어오면 아직 Goal 도착 안한 상태로 취급
        if (!PhotonNetwork.IsMasterClient) return;

        playersInGoal[newPlayer.ActorNumber] = false;
        BroadcastGoalUI();
    }
}
