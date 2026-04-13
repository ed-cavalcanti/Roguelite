using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 7f; // Valor menor para ser "rastreável" pelo olho
    public float lifeTime = 3f;
    public int damage =1;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Em vez de mover no Update, damos uma velocidade constante no início
        // transform.right é a direção para onde a bala "nasceu" apontando
        rb.linearVelocity = transform.right * speed;

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable hit = collision.GetComponent<IDamageable>();
        hit?.TakeDamage(damage);

        Debug.Log("Impacto com: " + collision.gameObject.name);
        Destroy(gameObject);
    }
}