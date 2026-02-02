using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeLineRenderer : MonoBehaviour
{
    private Transform playerA;
    private Transform playerB;
    private LineRenderer lr;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    private void LateUpdate()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Stage4")
        {
            lr.enabled = false;
            return;
        }
        lr.enabled = true;
        // 아직 연결 안 됐으면 계속 찾는다
        if (playerA == null || playerB == null)
        {
            TryBindPlayers();
            return;
        }

        lr.SetPosition(0, playerA.position);
        lr.SetPosition(1, playerB.position);
    }

    private void TryBindPlayers()
    {
        var players = FindObjectsByType<playerController>(FindObjectsSortMode.None);

        if (players == null || players.Length < 2) return;

        // 혹시라도 죽어있거나 disable된 애 걸러주기
        Transform a = null;
        Transform b = null;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] == null) continue;
            if (!players[i].gameObject.activeInHierarchy) continue;

            if (a == null) a = players[i].transform;
            else { b = players[i].transform; break; }
        }

        if (a == null || b == null) return;

        playerA = a;
        playerB = b;

        Debug.Log($"OK: {playerA.name} / {playerB.name}");
    }
}
