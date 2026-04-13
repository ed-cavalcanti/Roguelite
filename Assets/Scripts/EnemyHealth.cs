using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyFollow))]
public class EnemyHealth : MonoBehaviour, IDamageable
{
    public int health = 3;
    public int maxHealthCap = 12;
    private Animator anim;
    private bool isDead = false;
    private float maxHealthForPoints;

    void Start()
    {
        anim = GetComponent<Animator>();
        health = Mathf.Min(health, maxHealthCap);
        maxHealthForPoints = health;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Evita processar dano se já estiver a morrer

        health -= damage;
        StartCoroutine(FlashRed());

        if (health <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator FlashRed()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color originalColor = sr.color;
        sr.color = Color.red;
        yield return new WaitForSeconds(0.05f);
        sr.color = originalColor;
    }

    public void ScaleDifficulty(float multiplier)
    {
        health = Mathf.Min((int)(health * multiplier), maxHealthCap);
        maxHealthForPoints = health;

        if (TryGetComponent<EnemyFollow>(out var movement))
        {
            movement.speed *= 1 + (multiplier * 0.05f); // Aumenta a velocidade levemente
        }
    }

    void Die()
    {
        isDead = true;

        if (ScoreManager.instance != null)
        {
            ScoreManager.instance.AddScore(Mathf.RoundToInt(maxHealthForPoints));
        }

        if (GetComponent<EnemyFollow>() != null)
        {
            GetComponent<EnemyFollow>().enabled = false;
        }

        if (TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;
        }

        anim.SetTrigger("Die");
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 1f);
    }
}