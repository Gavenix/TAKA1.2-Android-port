using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    
    [SerializeField] private Waves[] waves;
    private int currentEnemyIndex;
    private int currentWaveIndex;
    private int enemiesLeftToSpawn;
    private bool isSpawning;
    private bool waveCompleted;

    private void Start()
    {
        StartNewWave();
    }

    private void Update()
    {
        // ���������, ������ �� ������ "E" � ��� �� ������ �� �����
        if (Input.GetKeyDown(KeyCode.E) && waveCompleted && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            LaunchWave();
        }
    }

    private IEnumerator SpawnEnemyInWave()
    {
        isSpawning = true;
        waveCompleted = false;

        while (enemiesLeftToSpawn > 0)
        {
            WaveSettings settings = waves[currentWaveIndex].WaveSettings[currentEnemyIndex];

            yield return new WaitForSeconds(settings.SpawnDelay);

            GameObject enemy = Instantiate(settings.Enemy, settings.NeededSpawner.transform.position, Quaternion.identity);
            enemy.tag = "Enemy"; // ������������� ��� ��� ������ ���������� ������

            enemiesLeftToSpawn--;
            currentEnemyIndex++;
        }

        isSpawning = false;
        waveCompleted = true; // ����� ���������, �� ����� �������� ������ ����� ����������� ���� ������
    }

    public void LaunchWave()
    {
        if (!isSpawning && currentWaveIndex < waves.Length - 1)
        {
            currentWaveIndex++;
            StartNewWave();
        }
    }

    private void StartNewWave()
    {
        currentEnemyIndex = 0;
        enemiesLeftToSpawn = waves[currentWaveIndex].WaveSettings.Length;
        StartCoroutine(SpawnEnemyInWave());
    }
}

[System.Serializable]
public class Waves
{
    [SerializeField] private WaveSettings[] waveSettings;
    public WaveSettings[] WaveSettings => waveSettings;
}

[System.Serializable]
public class WaveSettings
{
    [SerializeField] private GameObject enemy;
    public GameObject Enemy => enemy;

    [SerializeField] private GameObject neededSpawner;
    public GameObject NeededSpawner => neededSpawner;

    [SerializeField] private float spawnDelay;
    public float SpawnDelay => spawnDelay;
}


    