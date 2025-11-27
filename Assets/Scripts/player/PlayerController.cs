using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // Novo Input System

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    private PlayerStats playerStats;

    private Camera cam;


    [Header("Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Dash")]
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashDuration = 0.12f;
    [SerializeField] private float dashCooldown = 0.5f;

    [Header("Squash & Stretch")]    
    [SerializeField] private float squashAmount = 0.12f; 
    [SerializeField] private float squashSpeed = 5f;    

    private Vector3 originalScale;

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
        playerStats = GetComponent<PlayerStats>();

        rb = GetComponent<Rigidbody2D>();
        var playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        dashAction = playerInput.actions["Dash"];
    }

    void Start()
    {
        originalScale = transform.localScale;

        cam = Camera.main;

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
                0.08f 
            );
            transform.rotation = Quaternion.Euler(0, 0, currentAngle);
        }
        ApplySquashEffect();

        ClampToScreen();
    }



    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.linearVelocity = moveInput * moveSpeed;
        }

    }

    void ClampToScreen()
    {
        Vector3 pos = transform.position;

        Vector3 viewportPos = cam.WorldToViewportPoint(pos);

        viewportPos.x = Mathf.Clamp(viewportPos.x, 0.03f, 0.97f);
        viewportPos.y = Mathf.Clamp(viewportPos.y, 0.06f, 0.94f);

        transform.position = cam.ViewportToWorldPoint(viewportPos);
    }

    private void OnDashPerformed(InputAction.CallbackContext ctx)
    {
        if (!dashAvailable || isDashing) return;

        // BLOQUEIA O DASH SE NÃO TIVER AO MENOS 1 ENERGIA
        if (playerStats.currentEnergy < 1)
        {
            Debug.Log("Sem energia para dar dash!");
            return;
        }

        // GASTA 1 DE ENERGIA
        playerStats.UseEnergy(1);

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
    
    void ApplySquashEffect()
    {
        float moveMagnitude = moveInput.magnitude;

        
        Vector3 targetScale = originalScale;

        if (moveMagnitude > 0.01f)
        {
            
            float squashFactor = 1f - squashAmount;
            float stretchFactor = 1f + squashAmount;
            
            targetScale = new Vector3(
                originalScale.x * squashFactor,
                originalScale.y * stretchFactor,
                originalScale.z
            );
        }

        // transição suave entre escalas
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * squashSpeed
        );
    }
}
