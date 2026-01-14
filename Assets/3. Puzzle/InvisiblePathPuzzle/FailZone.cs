using UnityEngine;

public class FailZone : MonoBehaviour
{
    [SerializeField] InvisiblePathPuzzleController controller;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerController playerB = collision.GetComponent<playerController>();
        if (playerB == null) return;
        if (playerB.role != PlayerRole.B) return;
        if (collision.CompareTag("Player"))
        {
            controller.Fail(playerB);
        }
    }
}
