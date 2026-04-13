using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PortalInteraction : MonoBehaviour
{
    [Header("Dungeon")]
    public string dungeonScene;

    [Header("Mensagem ativa")]
    public GameObject UIMessage;

    private Animator anim;
    private bool isPlayerNear = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPlayerNear && Keyboard.current.eKey.wasPressedThisFrame)
        {
            goToDungeon();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
            UIMessage.SetActive(true);
            anim.SetBool("nearPlayer", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            UIMessage.SetActive(false);
            anim.SetBool("nearPlayer", false);
        }
    }

    void goToDungeon()
    {
        SceneManager.LoadScene(dungeonScene);
    }
}
