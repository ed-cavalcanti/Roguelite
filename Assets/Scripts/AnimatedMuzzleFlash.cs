using UnityEngine;
using System.Collections; // Necessário para Coroutines

[RequireComponent(typeof(SpriteRenderer))]
public class AnimatedEffect : MonoBehaviour
{
    [Header("Animação")]
    // Arraste a sequência de sprites para este array no Inspector
    public Sprite[] animationFrames;

    // Velocidade da animação (fps)
    public float framesPerSecond = 20f;

    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        // Começa a animação se houver frames
        if (animationFrames.Length > 0)
        {
            StartCoroutine(PlayAnimation());
        }
        else
        {
            // Se não tiver frames, destrói instantaneamente para evitar lixo na cena
            Destroy(gameObject);
        }
    }

    IEnumerator PlayAnimation()
    {
        // Calcula o tempo de espera entre cada frame
        float frameTime = 1f / framesPerSecond;

        // Passa por cada sprite no array
        for (int i = 0; i < animationFrames.Length; i++)
        {
            sr.sprite = animationFrames[i];

            // Espera o tempo do frame antes de ir para o próximo
            yield return new WaitForSeconds(frameTime);
        }

        // Após passar por todos os frames, destrói o objeto
        Destroy(gameObject);
    }
}