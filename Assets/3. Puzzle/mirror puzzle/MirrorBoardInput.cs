using UnityEngine;
using UnityEngine.InputSystem;

public class MirrorBoardInput : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private LayerMask mirrorLayer;
    [SerializeField] private float stepAngle = 15f;
    private Transform selectedPivot;

    private PlayerInput playerInput;
    private InputAction clickAction;

    public void SetPlayerInput(PlayerInput input)
    {
        ClearPlayerInput();

        playerInput = input;

        clickAction = playerInput.actions["Click"];
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
        Debug.Log("OnClick 들어옴");
        if (!ctx.performed) return;

        Vector3 mp = Mouse.current.position.ReadValue();
        mp.z = Mathf.Abs(cam.transform.position.z); 
        Vector3 worldPos3 = cam.ScreenToWorldPoint(mp);
        Vector2 worldPos = new Vector2(worldPos3.x, worldPos3.y);
        Collider2D hit = Physics2D.OverlapCircle(worldPos, 0.1f, mirrorLayer);
        if (hit == null) return;

        selectedPivot = hit.transform.GetComponentInParent<MirrorPivotTag>()?.transform;

        if (selectedPivot == null) return;


        selectedPivot.Rotate(0, 0, stepAngle);
     
    }
}

