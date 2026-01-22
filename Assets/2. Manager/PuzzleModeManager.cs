using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
public class PuzzleModeManager : MonoBehaviour
{
    public static PuzzleModeManager Instance;

    [SerializeField] private CinemachineCamera currentPuzzleCam;
    CinemachineCamera playerCam;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private MirrorBoardInput mirrorBoard;

    private void Awake()
    {
        Instance = this;
    }
 
    public void EnterPuzzleMode(CinemachineCamera puzzleCam, PlayerInput plInput)
    {
        playerCam = plInput.GetComponentInChildren<CinemachineCamera>();
        playerInput = plInput;
        playerInput.SwitchCurrentActionMap("MirrorPuzzle");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        currentPuzzleCam = puzzleCam;

        playerCam.Priority = 0;
        puzzleCam.Priority = 10;

        UIManager.Instance.OpenMirrorPuzzleHint();
        mirrorBoard.SetPlayerInput(playerInput);
        mirrorBoard.enabled = true;
    }
    public void ExitPuzzleModel()
    {
        playerInput.SwitchCurrentActionMap("Player");
       // Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;

        if (currentPuzzleCam != null) currentPuzzleCam.Priority = 0;

        playerCam.Priority = 10;

        mirrorBoard.ClearPlayerInput();
        mirrorBoard.enabled = false;
    }

}
