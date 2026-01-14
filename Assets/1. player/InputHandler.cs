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
        if (ctx.performed)
        {
            if (player.target == null) return;
            player.PlayerInteraction(player.target);
        }
    }
}
