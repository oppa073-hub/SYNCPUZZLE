using UnityEngine;
using Photon.Pun;

public class RopeConstraint2D : MonoBehaviourPun
{
    public float maxDistance = 5f;     // 줄 최대 길이
    public float stiffness = 35f;        // 당기는 힘
    public float damping = 8f;           // 흔들림 줄이기

    private Rigidbody2D rb;
    private Transform other;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        //씬에 있는 playerController 찾아서 내/상대 구분
        TryBindOther();


    }
    private void FixedUpdate()
    {
        // 내 물리는 내 것만
        if (!photonView.IsMine) return;
        if (other == null)
        {
            TryBindOther();
            return;
        }

        Vector2 a = rb.position;
        Vector2 b = other.position;

        Vector2 delta = a - b;
        float dist = delta.magnitude;
        if (dist <= maxDistance) return;

        Vector2 dir = delta / dist;             // b -> a 방향
        float excess = dist - maxDistance;      // 초과 거리

        // 1 당김 힘: 상대쪽으로 끌어당김 (-dir)
        Vector2 pull = -dir * (excess * stiffness);

        // 2 댐핑: 로프 방향으로 멀어지는 속도 성분을 줄여 튕김 방지
        float vAlong = Vector2.Dot(rb.linearVelocity, dir); // dir 방향 속도(멀어지는 방향)
        Vector2 damp = -dir * (vAlong * damping);

        rb.AddForce(pull + damp, ForceMode2D.Force);
    }
    private void TryBindOther()
    {
        var players = FindObjectsByType<playerController>(FindObjectsSortMode.None);
        for (int i = 0; i < players.Length; i++)
        {
            var p = players[i];
            if (p == null) continue;
            if (!p.gameObject.activeInHierarchy) continue;
            if (p.photonView == null) continue;
            if (p.photonView.IsMine) continue;

            other = p.transform;
            //Debug.Log($"[RopeConstraint] other bind OK: {other.name}");
            return;
        }
    }
}
