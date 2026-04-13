using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [Header("Configurações de Vida")]
    public int maxHealth = 6;
    private int currentHealth;
    public HealthDisplay healthDisplay;

    [Header("Invencibilidade")]
    public float invincibilityDuration = 1.5f;
    private bool isInvulnerable = false;
    public bool TakeDamageDisabled { get; set; }

    [Header("Feedback Visual")]
    public SpriteRenderer spriteRenderer;

    public UnityEvent OnDeath;
    public UnityEvent OnTakeDamage;

    public string onDeathScene;

    public GameOverManager gameOverManager;

    void Start()
    {
        currentHealth = maxHealth;
        if (healthDisplay != null)
        {
            healthDisplay.UpdateHearts(currentHealth, maxHealth);
        }

        if (gameOverManager == null)
        {
            gameOverManager = FindAnyObjectByType<GameOverManager>(FindObjectsInactive.Include);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable || TakeDamageDisabled) return;

        currentHealth -= damage;
        if (healthDisplay != null)
        {
            healthDisplay.UpdateHearts(currentHealth, maxHealth);
        }
        Debug.Log($"Vida do Gatinho: {currentHealth}");

        OnTakeDamage?.Invoke();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(BecomeInvulnerable());
        }
    }

    private IEnumerator BecomeInvulnerable()
    {
        isInvulnerable = true;

        if (spriteRenderer == null)
        {
            isInvulnerable = false;
            yield break;
        }

        // Efeito visual simples de piscar
        float timer = 0;
        while (timer < invincibilityDuration)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        spriteRenderer.enabled = true;
        isInvulnerable = false;
    }

    private void Die()
    {
        Debug.Log("O gatinho morreu!");

        if (gameOverManager == null)
        {
            gameOverManager = FindAnyObjectByType<GameOverManager>(FindObjectsInactive.Include);
        }

        int scoreConquistado = 0;
        if (ScoreManager.instance != null)
        {
            scoreConquistado = ScoreManager.instance.GetTotalScore();
        }

        if (gameOverManager != null)
        {
            gameOverManager.ShowGameOver(scoreConquistado);
        }
        else
        {
            Debug.LogError("GameOverManager não encontrado na cena. Verifique se existe um objeto com o script GameOverManager.");
        }

        gameObject.SetActive(false);
    }
}