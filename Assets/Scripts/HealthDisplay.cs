using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthDisplay : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;

    [Header("Configuração")]
    public GameObject heartPrefab;
    private readonly List<Image> hearts = new();

    // Chamado pelo PlayerHealth sempre que a vida mudar
    public void UpdateHearts(float currentHealth, float maxHealth)
    {
        // 1. Garantir que temos o número certo de objetos de coração na tela
        int totalHeartsNeeded = Mathf.CeilToInt(maxHealth / 2f);

        while (hearts.Count < totalHeartsNeeded)
        {
            GameObject newHeart = Instantiate(heartPrefab, transform);
            hearts.Add(newHeart.GetComponent<Image>());
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            // A vida referente a este coração específico (ex: coração 0 cuida da vida 1 e 2)
            float heartHealthValue = currentHealth - (i * 2);

            if (heartHealthValue >= 2)
            {
                hearts[i].sprite = fullHeart;
            }
            else if (heartHealthValue == 1)
            {
                hearts[i].sprite = halfHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
        }

        TriggerHeartAnimation();
    }

    private void TriggerHeartAnimation()
    {
        foreach (Image heartImage in hearts)
        {
            if (heartImage.TryGetComponent<Animator>(out var anim))
            {
                anim.SetTrigger("Hit");
            }
        }
    }
}