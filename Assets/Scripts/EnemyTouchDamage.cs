using UnityEngine;

public class EnemyTouchDamage : MonoBehaviour
{
    public int damage = 1;
    public float hitRadius = 1.1f;
    public Vector2 hitCenterOffset = new Vector2(0f, 0.8f);
    public LayerMask playerLayers;

    public void DealAttackDamage()
    {
        Vector2 hitCenter = GetHitCenter();
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitCenter, hitRadius, playerLayers);
        for (int i = 0; i < hits.Length; i++)
        {
            Collider2D hit = hits[i];
            if (hit == null) continue;

            TryDealDamage(hit.gameObject);
            break;
        }
    }

    private void TryDealDamage(GameObject target)
    {
        PlayerDodge dodge = target.GetComponentInParent<PlayerDodge>();
        if (dodge != null && dodge.isDashing) return;

        IDamageable player = target.GetComponentInParent<IDamageable>();
        if (player == null) return;

        player.TakeDamage(damage);
    }

    private Vector2 GetHitCenter()
    {
        return transform.TransformPoint(hitCenterOffset);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GetHitCenter(), hitRadius);
    }
}