using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    [Header("Configurações")]
    public TextMeshProUGUI scoreText;
    private int totalScore = 0;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = totalScore.ToString();
    }

    public int GetTotalScore()
    {
        return totalScore;
    }
}