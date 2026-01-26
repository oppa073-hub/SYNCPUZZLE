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
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private RuntimeAnimatorController animA;
    [SerializeField] private RuntimeAnimatorController animB;
    [SerializeField] private SpriteRenderer body;

    public NPCInteract currentNpc;
    private float groundCheckDistance = 0.1f;
    Rigidbody2D rigid;
    Animator animator;
    Vector2 moveDir;
    bool isGround;
    bool isInteract = false;
    bool isFaill = false;
    public IInteractable target;
    private bool facingRight = true;

    public event Action<ICommand> OnInteract;

    private void FixedUpdate()
    {
        Vector2 velocity = rigid.linearVelocity;
        velocity.x = moveDir.x * moveSpeed;
        rigid.linearVelocity = velocity;
    }
    private void Update()
    {
        if (!photonView.IsMine) return;
        if (animator == null) return;
        GroundCheck();                    

        float speed = Mathf.Abs(rigid.linearVelocity.x);
        animator.SetFloat("MoveSpeed", speed);
        animator.SetBool("IsGround", isGround);
    }
    private void LateUpdate()
    {
        if (!photonView.IsMine) return;


        if (moveDir.x > 0.01f) SetFacing(true);
        else if (moveDir.x < -0.01f) SetFacing(false);
    }

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        if (photonView.IsMine && mainCam == null) mainCam = Camera.main;
    }
    void Start()
    {
        if (!photonView.IsMine)
        {
            if (playerCamera) playerCamera.gameObject.SetActive(false);
            if (inputHandler) inputHandler.enabled = false;
            if (playerInput) playerInput.enabled = false;
        }
        else
        {
            if (playerCamera) playerCamera.gameObject.SetActive(true);
            if (inputHandler) inputHandler.enabled = true;
            if (playerInput) playerInput.enabled = true;

            role = PhotonNetwork.IsMasterClient ? PlayerRole.A : PlayerRole.B;
            photonView.RPC(nameof(RPC_SetRole), RpcTarget.AllBuffered, (int)role);
        }
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
    private void SetFacing(bool faceRight)
    {
        if (facingRight == faceRight) return;
        facingRight = faceRight;

        photonView.RPC(nameof(RPC_SetFacing), RpcTarget.All, facingRight);
    }

    [PunRPC]
    private void RPC_SetFacing(bool faceRight)
    {
        facingRight = faceRight;
        body.flipX = !facingRight;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interact"))
        {
            target = collision.GetComponentInParent<IInteractable>();
        }
        if (collision.CompareTag("Npc"))
        {
         
            currentNpc = collision.GetComponentInParent<NPCInteract>();
            Debug.Log($"currentNpc 세팅됨? {(currentNpc != null)}");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interact"))
        {
            var t = collision.GetComponentInParent<IInteractable>();
            if (t != null && t == target) target = null;
        }
        if (collision.CompareTag("Npc"))
        {
            var npc = collision.GetComponentInParent<NPCInteract>();
            if (npc != null && npc == currentNpc) currentNpc = null;
        }
    }
    [PunRPC]
    void RPC_SetRole(PlayerRole newRole)
    {
        role = newRole;
        ApplyViewByRole();   
    }
    void ApplyViewByRole()
    {
        if (animator == null) animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = (role == PlayerRole.A) ? animA : animB;
        if (body) body.sprite = null;

        if (!photonView.IsMine) return;
        if (role == PlayerRole.A) // 인간
        {
            mainCam.cullingMask &= ~(1 << LayerMask.NameToLayer("SafeTileOnly"));
        }
        else // 외계인
        {
            mainCam.cullingMask |= (1 << LayerMask.NameToLayer("SafeTileOnly"));
        }
    }
}

