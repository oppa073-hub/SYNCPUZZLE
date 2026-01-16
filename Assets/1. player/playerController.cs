using Photon.Realtime;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using Photon.Pun;
using Unity.Cinemachine;

public enum PlayerRole
{
    A, B
}

public class playerController : MonoBehaviourPun
{
    public PlayerRole role ;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] float jumpForce = 0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundChecker;
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private CinemachineCamera playerCamera;
    [SerializeField] private Camera mainCam;

    private float groundCheckDistance = 0.1f;
    Rigidbody2D rigid;
    Animator animator;
    Vector2 moveDir;
    bool isGround;
    bool isInteract = false;
    bool isFaill = false;
    public IInteractable target;

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
        if (mainCam == null) mainCam = Camera.main;

    }
    void Start()
    {
        if (!photonView.IsMine)
        {
            // 남의 캐릭터
            playerCamera.gameObject.SetActive(false);
            inputHandler.enabled = false;
        }
        else
        {
            // 내 캐릭터
            playerCamera.gameObject.SetActive(true);
            inputHandler.enabled = true;
        }
        if (!photonView.IsMine) return;

        if (PhotonNetwork.IsMasterClient)
            role = PlayerRole.A;
        else
            role = PlayerRole.B;
        ApplyViewByRole();
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
    public void PlayerInteraction(IInteractable target)
    {
        ICommand cmd = new InteractionCommand(target,this);
        OnInteract?.Invoke(cmd);
        
    }

    private void GroundCheck()
    {
        isGround = Physics2D.Raycast(
           groundChecker.position,
           Vector2.down,
           groundCheckDistance,
           groundLayer);


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Interact")) return;
        target = collision.GetComponentInParent<IInteractable>();
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Interact")) return;
    
        var t = collision.GetComponentInParent<IInteractable>();
        if (t != null && t == target) target = null;
    }
    void ApplyViewByRole()
    {
        if (role == PlayerRole.A)
        {
            mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("SafeTileOnly"));
        }
        else // B
        {
            mainCam.cullingMask |= (1 << LayerMask.NameToLayer("SafeTileOnly"));
        }
    }
}

