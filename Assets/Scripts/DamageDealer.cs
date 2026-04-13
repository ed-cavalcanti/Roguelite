using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int damageAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerDodge dodge = collision.GetComponentInParent<PlayerDodge>();
        if (dodge != null && dodge.isDashing) return;

        IDamageable hit = collision.GetComponentInParent<IDamageable>();

        hit?.TakeDamage(damageAmount);
    }
}