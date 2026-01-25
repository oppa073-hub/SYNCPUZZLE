using Photon.Pun;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class MirrorPuzzleZoneTrigger : MonoBehaviour
{
    [SerializeField] private CinemachineCamera puzzleCam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var pv = collision.GetComponentInParent<PhotonView>();
            if (pv == null || !pv.IsMine) return;
            var input = collision.GetComponentInParent<PlayerInput>();
            Debug.Log("dd");
            PuzzleModeManager.Instance.EnterPuzzleMode(puzzleCam, input);
        }
       
    }

}
