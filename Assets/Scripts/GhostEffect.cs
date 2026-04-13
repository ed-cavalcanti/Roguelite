using UnityEngine;

public class GhostEffect : MonoBehaviour
{
    private SpriteRenderer sr;
    public float fadeSpeed = 5f;
    private Color color;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        color = sr.color;
    }

    void Update()
    {
        color.a -= fadeSpeed * Time.deltaTime;
        sr.color = color;

        if (color.a <= 0)
        {
            Destroy(gameObject);
        }
    }
}