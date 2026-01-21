using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;
public class MirrorPuzzleZoneTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineCamera puzzleCam;
    PlayerInput playerInput;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInput = collision.GetComponent<PlayerInput>();
            Debug.Log("dd");
            PuzzleModeManager.Instance.EnterPuzzleMode(puzzleCam, playerInput);
        }
       
    }

}
