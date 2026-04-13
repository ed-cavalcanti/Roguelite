using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    public int damage = 1;
    public float damageInterval = 1f;

    private float nextDamageTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDealDamage(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDealDamage(collision.gameObject);
    }

    private void TryDealDamage(GameObject target)
    {
        if (!target.CompareTag("Player")) return;
        if (Time.time < nextDamageTime) return;

        PlayerDodge dodge = target.GetComponentInParent<PlayerDodge>();
        if (dodge != null && dodge.isDashing) return;

        IDamageable player = target.GetComponentInParent<IDamageable>();
        if (player == null) return;

        player.TakeDamage(damage);
        nextDamageTime = Time.time + Mathf.Max(0.01f, damageInterval);
    }
}