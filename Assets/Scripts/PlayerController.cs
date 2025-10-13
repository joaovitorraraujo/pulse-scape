using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // Novo Input System

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.12f;
    [SerializeField] private float dashCooldown = 0.5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.up;

    private InputAction moveAction;
    private InputAction dashAction;

    private bool isDashing = false;
    private bool dashAvailable = true;

    private float currentAngle;
    private float angleVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        var playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        dashAction = playerInput.actions["Dash"];
    }

    void OnEnable()
    {
        moveAction.Enable();
        dashAction.Enable();
        dashAction.performed += OnDashPerformed;
    }

    void OnDisable()
    {   
        dashAction.performed -= OnDashPerformed;
        dashAction.Disable();
        moveAction.Disable();
    }

    void Update()
    {
        if (!isDashing)
        {
            moveInput = moveAction.ReadValue<Vector2>();
            if (moveInput.sqrMagnitude > 1f) moveInput = moveInput.normalized;

            if (moveInput.sqrMagnitude > 0.001f)
                lastMoveDirection = moveInput.normalized;
        }
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
        if (!isDashing)
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }
    }

    private void OnDashPerformed(InputAction.CallbackContext ctx)
    {
        if (!dashAvailable || isDashing) return;

        Vector2 dashDir = moveInput.sqrMagnitude > 0.01f ? moveInput.normalized : lastMoveDirection;
        if (dashDir.sqrMagnitude <= 0.01f) dashDir = Vector2.up; 

        StartCoroutine(DoDash(dashDir));
    }
    
    private IEnumerator DoDash(Vector2 dir)
    {
        isDashing = true;
        dashAvailable = false;

        
        Vector2 previousVelocity = rb.linearVelocity;

        
        rb.linearVelocity = dir * dashSpeed;

        float t = 0f;
        while (t < dashDuration)
        {
            rb.linearVelocity = dir * dashSpeed;
            t += Time.deltaTime;
            yield return null;
        }

        
        isDashing = false;

        
        rb.linearVelocity = previousVelocity;

        
        if (dashCooldown > 0f)
        {
            float cd = 0f;
            while (cd < dashCooldown)
            {
                cd += Time.deltaTime;
                yield return null;
            }
        }

        dashAvailable = true;
    }
}
