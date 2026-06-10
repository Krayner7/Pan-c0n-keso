using Unity.Netcode;
using UnityEngine;
public class PlayerController : NetworkBehaviour
{
    [Header("Movimiento")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    private CharacterController controller;
    private PlayerInputActions input;

    private Vector2 moveInput;
    private float yVelocity;

    private bool isSprinting;
    private bool jumpRequested;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    private bool isGrounded;

    [SerializeField] private Transform cameraTarget;


    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        input.Player.Enable();

        input.Player.Move.performed +=
            ctx => moveInput = ctx.ReadValue<Vector2>();

        input.Player.Move.canceled +=
            ctx => moveInput = Vector2.zero;

        input.Player.Sprint.performed +=
            _ => isSprinting = true;

        input.Player.Sprint.canceled +=
            _ => isSprinting = false;

        input.Player.Jump.performed +=
            _ => jumpRequested = true;

        CameraOrbit cam =
            Camera.main.GetComponent<CameraOrbit>();

        cam.follow = cameraTarget;
        cam.player = this;

        cam.ActivateCamera();
    }

    private void OnEnable() { }
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        input = new PlayerInputActions();
    }

    private void OnDisable()
    {
        input.Player.Disable();
    }

    private void Update()
    {
        if (!IsOwner) return;

        HandleMovement();
    }
    public Vector2 GetLookInput()
    {
        return input.Player.Look.ReadValue<Vector2>();
    }
    private void HandleMovement()
    {
        //  Movimiento horizontal (relativo al player)
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
        move = transform.TransformDirection(move);

        float speed = isSprinting ? sprintSpeed : walkSpeed;

        // Comprobar suelo
        isGrounded = Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundLayer
        );

        // Mantener pegado al suelo
        if (isGrounded && yVelocity < 0)
        {
            yVelocity = -2f;
        }

        // Salto
        if (isGrounded && jumpRequested)
        {
            yVelocity = 0f;
            yVelocity += jumpForce;

            jumpRequested = false;
        }

        //  Gravedad
        yVelocity += gravity * Time.deltaTime;

        //  Movimiento final
        Vector3 velocity = move * speed;
        velocity.y = yVelocity;

        controller.Move(velocity * Time.deltaTime);

        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
