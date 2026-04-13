using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 1;
    public float damageInterval = 1f;

    private float nextDamageTime;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryDealDamage(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryDealDamage(collision.gameObject);
    }

    private void TryDealDamage(GameObject target)
    {
        if (!target.CompareTag("Player")) return;
        if (Time.time < nextDamageTime) return;

        PlayerDodge dodge = target.GetComponentInParent<PlayerDodge>();
        if (dodge != null && dodge.isDashing) return;

        IDamageable hit = target.GetComponentInParent<IDamageable>();
        if (hit == null) return;

        hit.TakeDamage(damageAmount);
        nextDamageTime = Time.time + Mathf.Max(0.01f, damageInterval);
    }
}