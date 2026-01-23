using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.GraphicsBuffer;
public class InputHandler : MonoBehaviour
{
    [SerializeField] playerController player;
    Vector2 direction;

    public void OnMove(InputAction.CallbackContext ctx)
    {
        direction = ctx.ReadValue<Vector2>();
        player.PlayerMove(direction);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
            player.PlayerJump();
    }

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        if (player.target != null) player.PlayerInteraction(player.target);
        if (player.currentNpc != null) player.currentNpc.TryTalk();
        else Debug.Log("currentNpc = null");
        
    }
    private void OnEnable()
    {
        Debug.Log("InputHandler Enable");
    }

}
