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

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        playerCam = playerPrefab.GetComponentInChildren<CinemachineCamera>();
    }
    public void EnterPuzzleMode(CinemachineCamera puzzleCam, PlayerInput plInput)
    {
        playerInput = plInput;
        playerInput.SwitchCurrentActionMap("MirrorPuzzle");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        currentPuzzleCam = puzzleCam;

        playerCam.Priority = 0;
        puzzleCam.Priority = 20;

        UIManager.Instance.OpenMirrorPuzzleHint();

    }
    public void ExitPuzzleModel()
    {
        playerInput.SwitchCurrentActionMap("Player");
       // Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (currentPuzzleCam != null) currentPuzzleCam.Priority = 0;

        playerCam.Priority = 10;
    }

}
