using UnityEngine;
using UnityEngine.InputSystem; // Novo Input System

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    private InputAction moveAction;

    private float currentAngle;
    private float angleVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        var playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"]; // "Move" = nome da Action no InputActions
    }

    void OnEnable()
    {
        moveAction.Enable();
    }

    void OnDisable()
    {
        moveAction.Disable();
    }

    void Update()
    {
        // Lê o input a cada frame
        moveInput = moveAction.ReadValue<Vector2>();
        if (moveInput.sqrMagnitude > 1f)
            moveInput = moveInput.normalized;
    }

    void LateUpdate()
    {
    if (moveInput.sqrMagnitude > 0.01f)
    {
        float targetAngle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg - 90f;
        currentAngle = Mathf.SmoothDampAngle(
            currentAngle,
            targetAngle,
            ref angleVelocity,
            0.08f // tempo para suavizar totalmente (ajuste)
        );
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
    }   



    void FixedUpdate()
    {
        // Aplica o movimento com física
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
