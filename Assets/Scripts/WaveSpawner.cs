using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public float spawnRate = 1f;
}

[System.Serializable]
public class Wave
{
    public string waveName;
    public List<EnemySpawnData> enemies;
}

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private UpgradeManager upgradeManager;
    public List<Wave> waves;
    public Transform player;
    public float spawnRadius = 10f;
    public Animator shopAnimator;

    [Header("Игровые границы")]
    public float minX, maxX, minY, maxY;

    [Header("Tilemap для спавна врагов")]
    public Tilemap groundTilemap;

    [Header("Проверка коллайдеров")]
    public LayerMask obstacleLayer; // Слой препятствий

    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI waveText;

    private int currentWaveIndex = -1;
    private bool isSpawning = false;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    

    private int waveTime = 30;
    private bool waveRunning = false;

    private void Start()
    {
        Debug.Log($"Границы уровня: minX = {minX}, maxX = {maxX}, minY = {minY}, maxY = {maxY}");
        UpdateUI();

        StartWave();
    }

    private void Update()
    {
        if (!waveRunning && Input.GetKeyDown(KeyCode.Y) && spawnedEnemies.Count == 0)
        {
            StartWave();
        }

        spawnedEnemies.RemoveAll(enemy => enemy == null);
    }

    public void StartWave()
    {
        if (currentWaveIndex + 1 >= waves.Count) return;

        StopAllCoroutines();
        waveRunning = true;
        waveTime = 30;
        isSpawning = true;

        timeText.gameObject.SetActive(true);
        waveText.gameObject.SetActive(true);

        timeText.color = Color.black;

        currentWaveIndex++;
        UpdateUI();

        StartCoroutine(WaveTimer());
        StartCoroutine(SpawnWave(waves[currentWaveIndex]));
    }

    IEnumerator WaveTimer()
    {
        while (waveRunning)
        {
            yield return new WaitForSeconds(1f);
            waveTime--;

            if (waveTime <= 5)
            {
                timeText.color = Color.red;
            }

            UpdateUI();

            if (waveTime <= 0)
            {
                WaveComplete();
            }
        }
    }

    void WaveComplete()
    {
        StopAllCoroutines();
        ClearAllEnemies();
        waveRunning = false;
        isSpawning = false;
        waveTime = 0;

        UpdateUI();

        if (shopAnimator != null)
        {
            shopAnimator.ResetTrigger("Hide");
            shopAnimator.SetTrigger("Show");
        }

        var player = FindObjectOfType<PlayerController>();
        if (player != null && player.pendingLevelUp)
        {
            Debug.Log("[Wave] Showing upgrades after wave complete");

            if (upgradeManager != null)
            {
                upgradeManager.ShowUpgradeOptions();
                player.pendingLevelUp = false;
            }
            else Debug.LogError("[Wave] UpgradeManager is NULL!");
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        isSpawning = true;

        foreach (EnemySpawnData enemyData in wave.enemies)
        {
            StartCoroutine(SpawnEnemyLoop(enemyData));
        }

        yield return null;
    }

    IEnumerator SpawnEnemyLoop(EnemySpawnData enemyData)
    {
        while (isSpawning)
        {
            GameObject enemy = SpawnEnemy(enemyData.enemyPrefab);
            if (enemy != null)
            {
                spawnedEnemies.Add(enemy);
            }
            yield return new WaitForSeconds(enemyData.spawnRate);
        }
    }

    GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        if (player == null || groundTilemap == null) return null;

        Vector2 spawnPosition;
        int attempts = 15; // Больше попыток для поиска подходящей позиции

        do
        {
            float randomAngle = Random.Range(0f, 2f * Mathf.PI);
            float randomX = player.position.x + Mathf.Cos(randomAngle) * spawnRadius;
            float randomY = player.position.y + Mathf.Sin(randomAngle) * spawnRadius;

            spawnPosition = new Vector2(randomX, randomY);
            attempts--;
        }
        while ((!IsInsideBounds(spawnPosition) || !IsOnTilemap(spawnPosition) || IsPositionBlocked(spawnPosition)) && attempts > 0);

        if (!IsInsideBounds(spawnPosition) || !IsOnTilemap(spawnPosition) || IsPositionBlocked(spawnPosition))
        {
            Debug.LogWarning("Не удалось найти безопасное место для спавна врага!");
            return null;
        }

        return Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    bool IsInsideBounds(Vector2 position)
    {
        return position.x >= minX && position.x <= maxX && position.y >= minY && position.y <= maxY;
    }

    bool IsOnTilemap(Vector2 position)
    {
        Vector3Int tilePosition = groundTilemap.WorldToCell(position);
        return groundTilemap.HasTile(tilePosition);
    }

    bool IsPositionBlocked(Vector2 position)
    {
        return Physics2D.OverlapCircle(position, 0.5f, obstacleLayer) != null;
    }

    void ClearAllEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }
        spawnedEnemies.Clear();
    }

    void UpdateUI()
    {
        if (timeText == null || waveText == null)
        {
            Debug.LogWarning("Text UI не назначен в инспекторе!");
            return;
        }

        timeText.text = waveTime.ToString();
        waveText.text = waveRunning ? "Wave: " + (currentWaveIndex + 1) : "Press 'E' for next Wave!";
    }
}
