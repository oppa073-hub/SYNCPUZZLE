using UnityEngine;

public class GoalZone : MonoBehaviour
{ 
    [SerializeField] InvisiblePathPuzzleController controller;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerController playerB = collision.GetComponent<playerController>();
        if (playerB == null) return;
        if (playerB.role != PlayerRole.B) return;
        if (playerB == null) return;
        
        controller.SetSolved(true);
    }
}
