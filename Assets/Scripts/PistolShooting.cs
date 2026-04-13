using UnityEngine;
using UnityEngine.InputSystem;

public class PistolShooting : MonoBehaviour
{
    [Header("Referências")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Áudio")]
    public AudioSource audioSource; // Arraste o componente AudioSource aqui
    public AudioClip shootSound;    // Arraste o arquivo de som (.wav ou .mp3) aqui
    [Range(0, 1)] public float volume = 0.5f;

    [Header("Configurações")]
    public float fireRate = 0.8f;
    public float minFireRate = 0.4f;
    private float nextFireTime;
    private DungeonManager dungeonManager;

    void Start()
    {
        dungeonManager = FindAnyObjectByType<DungeonManager>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            Shoot();
            float currentDifficultyMultiplier = dungeonManager != null ? dungeonManager.CurrentDifficultyMultiplier : 1f;
            float currentFireRate = Mathf.Max(minFireRate, fireRate / currentDifficultyMultiplier);
            nextFireTime = Time.time + currentFireRate;
        }
    }

    void Shoot()
    {
        // 1. Instanciar Projétil
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        // 2. Instanciar Efeito Visual (Muzzle Flash)
        if (muzzleFlashPrefab != null)
        {
            Instantiate(muzzleFlashPrefab, firePoint.position, firePoint.rotation, firePoint);
        }

        // 3. Tocar Som de Disparo
        PlayShootSound();
    }

    void PlayShootSound()
    {
        if (audioSource != null && shootSound != null)
        {
            // O SEGREDO DO ROGUELITE: Variar o pitch levemente
            // Isso faz com que cada tiro soe um pouco diferente, evitando fadiga auditiva
            audioSource.pitch = Random.Range(0.8f, 1.1f);

            // PlayOneShot permite que os sons se sobreponham se você atirar rápido
            audioSource.PlayOneShot(shootSound, volume);
        }
    }
}