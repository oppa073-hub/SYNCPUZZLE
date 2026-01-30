using UnityEngine;
using UnityEngine.InputSystem;

public class MirrorBoardInput : MonoBehaviour
{
    [SerializeField] private Camera puzzleCam;
    [SerializeField] private LayerMask mirrorHitLayer;
    [SerializeField] private float clickRadius = 0.15f;
    [SerializeField] private AudioClip mirrorSfx;
    int puzzleId = 5;
    private PlayerInput playerInput;
    private InputAction clickAction;
    private bool isActiveBoard;
    public void SetPlayerInput(PlayerInput input)
    {
        ClearPlayerInput();

        playerInput = input;

        clickAction = playerInput.actions["Click"];
        clickAction.performed -= OnClick;
        clickAction.performed += OnClick;
    }
    public void ClearPlayerInput()
    {
        if (clickAction != null)
        {
            clickAction.performed -= OnClick;
            clickAction = null;
        }
        playerInput = null;
    }
    private void OnDisable()
    {
        ClearPlayerInput();
    }
    public void OnClick(InputAction.CallbackContext ctx)
    {
        if (!ctx.ReadValueAsButton()) return;
        if (!isActiveBoard) return;
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos.z = Mathf.Abs(puzzleCam.transform.position.z);
        Vector2 worldPos = puzzleCam.ScreenToWorldPoint(mousePos);
        Collider2D hit = Physics2D.OverlapCircle(worldPos, clickRadius, mirrorHitLayer);
        if (hit == null) return;

        MirrorController mirror = hit.GetComponentInParent<MirrorController>();
        if (mirror == null) return;

        // 마스터에게 회전 요청
        PuzzleManager.Instance.RequestPress(puzzleId, 0, mirror.mirrorIndex);
    }
    public void SetActiveBoard(bool active)
    {
        isActiveBoard = active;
    }
}

