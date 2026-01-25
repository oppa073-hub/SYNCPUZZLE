using Photon.Pun;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        var pc = collision.GetComponent<playerController>();
        if (pc == null) return;

        if (!pc.photonView.IsMine) return; // 내 로컬 플레이어만 보고
        StageManager.instance.ReportGoalState(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        var pc = collision.GetComponent<playerController>();
        if (pc == null) return;

        if (!pc.photonView.IsMine) return; // 내 로컬 플레이어만 보고
        StageManager.instance.ReportGoalState(false);
    }
}
