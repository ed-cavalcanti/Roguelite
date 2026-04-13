using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // OBRIGATÓRIO

[RequireComponent(typeof(PlayerHealth))]
public class PlayerDodge : MonoBehaviour
{
    [Header("Configurações de Dash")]
    public float dashDistance = 5f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Efeito de Rastro")]
    public GameObject ghostPrefab;
    public float ghostDelay = 0.05f;

    public bool isDashing;
    private bool canDash = true;
    private Rigidbody2D rb;
    private SpriteRenderer playerSR;
    private PlayerHealth playerHealth;

    [Header("Layers")]
    [SerializeField] private string playerLayerName = "Player";
    [SerializeField] private string enemyLayerName = "Enemies";

    // Referências do Novo Input System
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction dashAction;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerSR = GetComponent<SpriteRenderer>();
        playerInput = GetComponent<PlayerInput>();
        playerHealth = GetComponent<PlayerHealth>();

        // "Linka" as variáveis com as ações criadas no Asset
        moveAction = playerInput.actions["Move"];
        dashAction = playerInput.actions["Dash"];
    }

    void OnEnable()
    {
        // Subscreve ao evento de "clique" do Dash
        dashAction.performed += OnDashPerformed;
    }

    void OnDisable()
    {
        // Limpa a subscrição ao desativar o objeto
        dashAction.performed -= OnDashPerformed;

        // Garante restauração de estado caso o objeto seja desativado durante o dash.
        RestoreDashDamageState();
    }

    private void RestoreDashDamageState()
    {
        if (playerHealth != null) playerHealth.TakeDamageDisabled = false;

        int playerLayer = LayerMask.NameToLayer(playerLayerName);
        int enemyLayer = LayerMask.NameToLayer(enemyLayerName);
        if (playerLayer != -1 && enemyLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        }
    }

    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        // Se puder dar dash e não estiver no meio de um...
        if (canDash && !isDashing)
        {
            Vector2 moveInput = moveAction.ReadValue<Vector2>();

            if (moveInput == Vector2.zero)
            {
                moveInput = playerSR.flipX ? Vector2.left : Vector2.right;
            }

            StartCoroutine(PerformDash(moveInput.normalized));
        }
    }

    IEnumerator PerformDash(Vector2 direction)
    {
        canDash = false;
        isDashing = true;

        if (playerHealth != null) playerHealth.TakeDamageDisabled = true;

        int playerLayer = LayerMask.NameToLayer(playerLayerName);
        int enemyLayer = LayerMask.NameToLayer(enemyLayerName);

        if (playerLayer != -1 && enemyLayer != -1)
        {
            Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);
        }
        else
        {
            Debug.LogWarning($"PlayerDodge: layer inválida para ignorar colisão ({playerLayerName} x {enemyLayerName}).");
        }

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        rb.linearVelocity = direction * (dashDistance / dashDuration);

        StartCoroutine(SpawnGhostRoutine());

        yield return new WaitForSeconds(dashDuration);

        RestoreDashDamageState();

        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = originalGravity;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    IEnumerator SpawnGhostRoutine()
    {
        while (isDashing)
        {
            GameObject ghost = Instantiate(ghostPrefab, transform.position, transform.rotation);
            SpriteRenderer ghostSR = ghost.GetComponent<SpriteRenderer>();
            ghostSR.sprite = playerSR.sprite;
            ghostSR.flipX = playerSR.flipX;

            yield return new WaitForSeconds(ghostDelay);
        }
    }
}