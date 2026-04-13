using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // Essencial para mudar de cena
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [Header("Referências de UI")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI finalScoreText;
    public string initialIslandSceneName = "StartIsland";

    public void ShowGameOver(int finalScore)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogError("GameOverPanel não está atribuído no GameOverManager.");
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = finalScore.ToString();
        }
        Time.timeScale = 0f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReturnToIsland()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(initialIslandSceneName);
    }
}