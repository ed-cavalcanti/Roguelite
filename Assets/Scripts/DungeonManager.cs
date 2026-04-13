using UnityEngine;
using System.Collections;

public class DungeonManager : MonoBehaviour
{
    [Header("Configurações de Spawn")]
    public GameObject enemyPrefab;
    public Transform player;
    public float initialSpawnInterval = 3f;
    public float minSpawnInterval = 0.5f;

    [Header("Configurações de Layer")]
    public string enemyLayerName = "Enemies"; // Nome da Layer que você criou no Unity
    public LayerMask obstacleLayer; // Layer das Paredes para evitar spawn dentro delas

    [Header("Configurações de Raio")]
    public float minSpawnRadius = 3f;
    public float maxSpawnRadius = 8f;

    [Header("Escalonamento de Dificuldade")]
    public float difficultyScaleRate = 0.05f;
    private float currentTime = 0f;
    private float currentDifficultyMultiplier = 1f;
    public float CurrentDifficultyMultiplier => currentDifficultyMultiplier;

    [Header("Segurança de Spawn")]
    public int maxSpawnAttempts = 10;

    void Start()
    {
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        currentDifficultyMultiplier = 1f + (currentTime * difficultyScaleRate);
    }

    IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            float currentInterval = Mathf.Max(minSpawnInterval, initialSpawnInterval / currentDifficultyMultiplier);
            yield return new WaitForSeconds(currentInterval);
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPos = Vector3.zero;
        bool validPositionFound = false;
        int attempts = 0;

        while (!validPositionFound && attempts < maxSpawnAttempts)
        {
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float randomDistance = Random.Range(minSpawnRadius, maxSpawnRadius);
            spawnPos = player.position + new Vector3(randomDirection.x * randomDistance, randomDirection.y * randomDistance, 0);

            // Verifica se a posição de spawn está dentro de uma parede
            Collider2D hit = Physics2D.OverlapCircle(spawnPos, 0.3f, obstacleLayer);
            if (hit == null) validPositionFound = true;
            attempts++;
        }

        if (validPositionFound)
        {
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

            // --- CORREÇÃO DE LAYER ---
            // Força o inimigo a estar na Layer correta
            int layerIndex = LayerMask.NameToLayer(enemyLayerName);
            if (layerIndex != -1)
            {
                newEnemy.layer = layerIndex;
            }
            else
            {
                Debug.LogWarning($"A Layer '{enemyLayerName}' não foi encontrada! Verifique o nome no Unity.");
            }

            if (newEnemy.TryGetComponent<EnemyHealth>(out EnemyHealth health))
            {
                health.ScaleDifficulty(currentDifficultyMultiplier);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (player == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, minSpawnRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(player.position, maxSpawnRadius);
    }
}