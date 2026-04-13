using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class EnemyFollow : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float speed = 3f;
    public float detectionRange = 10f; // Distância que ele começa a seguir
    public float stopDistance = 0.8f;  // Para não ficar "dentro" do player

    [Header("Configurações de Ataque")]
    public float attackRangeMultiplier = 1.5f;
    public float attackDuration = 0.45f;
    public float attackCooldown = 1f;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private float attackEndTime;
    private float nextAttackTime;
    private bool isAttacking;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (isAttacking && Time.time >= attackEndTime)
        {
            isAttacking = false;
        }

        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Speed", 0f);
            return;
        }

        if (distance < detectionRange && distance > stopDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            rb.linearVelocity = direction * speed;

            anim.SetFloat("Speed", 1f);
            spriteRenderer.flipX = direction.x < 0;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Speed", 0f);
        }

        if (distance <= attackRangeMultiplier * stopDistance && Time.time >= nextAttackTime)
        {
            isAttacking = true;
            attackEndTime = Time.time + attackDuration;
            nextAttackTime = Time.time + attackCooldown;
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Speed", 0f);
            anim.SetTrigger("Attack");
        }
    }
}