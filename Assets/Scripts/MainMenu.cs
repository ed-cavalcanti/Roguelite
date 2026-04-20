using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string startSceneName = "StartIsland";
    public LevelLoader levelLoader;

    public void PlayGame()
    {
        levelLoader.LoadNextLevel(startSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("O gato saiu do jogo!");
        Application.Quit();
    }
}