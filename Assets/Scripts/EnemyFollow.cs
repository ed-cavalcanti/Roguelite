using UnityEngine;
using System.Collections;

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
    public float attackHitDelay = 0.12f;

    private Transform player;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private float attackEndTime;
    private float nextAttackTime;
    private bool isAttacking;
    private int currentAttackId;
    private EnemyTouchDamage touchDamage;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) player = playerObj.transform;

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        touchDamage = GetComponent<EnemyTouchDamage>();

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

        float effectiveStopDistance = touchDamage != null ? 0.05f : stopDistance;
        float attackTriggerDistance = attackRangeMultiplier * stopDistance;

        if (distance < detectionRange && distance > effectiveStopDistance)
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

        if (distance <= attackTriggerDistance && Time.time >= nextAttackTime)
        {
            isAttacking = true;
            currentAttackId++;
            attackEndTime = Time.time + attackDuration;
            // O próximo ataque só libera após terminar o atual + cooldown.
            nextAttackTime = attackEndTime + attackCooldown;
            rb.linearVelocity = Vector2.zero;
            anim.SetFloat("Speed", 0f);
            anim.SetTrigger("Attack");
            StartCoroutine(DealAttackDamageWithDelay(currentAttackId));
        }
    }

    private IEnumerator DealAttackDamageWithDelay(int attackId)
    {
        float delay = Mathf.Max(0f, attackHitDelay);
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }

        // Evita aplicar hit de uma coroutine de ataque antiga.
        if (attackId == currentAttackId)
        {
            touchDamage?.DealAttackDamage();
        }
    }
}