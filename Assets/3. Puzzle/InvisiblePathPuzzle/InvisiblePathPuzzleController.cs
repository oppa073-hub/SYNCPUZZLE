using UnityEngine;
using System.Collections;
public class InvisiblePathPuzzleController : MonoBehaviour
{
    [SerializeField] Transform respawnPoint;
    public void Fail(playerController player)
    {
        var rig = player.GetComponent<Rigidbody>();
        rig.linearVelocity = Vector2.zero;
        rig.angularVelocity = Vector2.zero;
        player.transform.position = respawnPoint.position;
        StartCoroutine(BlinkCoroutine(player));
    }
    private IEnumerator BlinkCoroutine(playerController player)
    {
        var playerSp = player.GetComponentInChildren<SpriteRenderer>();
        Color original = playerSp.color;
        Color blinkColor = original;
        blinkColor.a = 0.3f;   // 살짝 투명

        playerSp.color = blinkColor;
        yield return new WaitForSeconds(0.1f);

        playerSp.color = original;
        yield return new WaitForSeconds(0.1f);

        // 무적 끝나면 원래색 고정
        playerSp.color = original;
    }

    public void SetSolved(bool solved)
    {
        if (solved)
        {
            //합격 처리
            //다른플레이어도 모두 통과시키는거
        }
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //   var playerB = collision.GetComponent<playerController>();
    // 
    //   if (playerB.role != PlayerRole.B) return;
    //
    //    if (collision.CompareTag("FailZone"))
    //    {
    //        playerB.Fail(respawn);
    //    }
    //    if (collision.CompareTag("GoalZone"))
    //    {
    //        //성공
    //    }
    //}
}
