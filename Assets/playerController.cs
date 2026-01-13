using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class playerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float jumpForce = 0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundChecker;

    private float groundCheckDistance = 0.1f;
    Rigidbody2D rigid;
    Animator animator;
    Vector2 moveDir;
    bool isGround;

    public event Action<ICommand> OnInteract;

    private void FixedUpdate()
    {
        Vector2 velocity = rigid.linearVelocity;
        velocity.x = moveDir.x * moveSpeed;
        rigid.linearVelocity = velocity;
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void PlayerMove(Vector2 dir)
    {
        moveDir = dir;

        // animator.SetFloat("MoveFloat", dir.magnitude);
    }
    public void PlayerJump()
    {
        GroundCheck();
        if (isGround)
        {
            rigid.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }

    }
    // public void PlayerInteraction(IInteractable target)
    // {
    //     ICommand cmd = new InteractCommand(target);
    //     OnInteract?.Invoke(cmd);
    // }

    private void GroundCheck()
    {
        isGround = Physics2D.Raycast(
           groundChecker.position,
           Vector2.down,
           groundCheckDistance,
           groundLayer);


    }
}

