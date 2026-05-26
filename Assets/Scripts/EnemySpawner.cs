using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemies to spawn")]
    [SerializeField] private GameObject walker;
    [SerializeField] private GameObject runner;
    [SerializeField] private GameObject heavy;
    [SerializeField] private GameObject harpy;
    [SerializeField] private GameObject fatty;
    [SerializeField] private GameObject spitter;

    [Header("Spawn Positions")]
    [SerializeField] private List<Transform> spawnPositions;

    [Header("Waves configuration")]
    [SerializeField] private int baseEnemiesPerWave = 10;

    [Header("Events")]
    [SerializeField] private StartWaveEvent startWaveEvent;
    [SerializeField] private FinishWaveEvent finishWaveEvent;
    [SerializeField] private DeathEnemy deathEnemyEvent;
    [SerializeField] private UnitRecruitedEvent unitRecruitedEvent;

    private int currentWave;
    private Transform selectedSpawnPosition;
    private DificultyMode dificultyMode;
    int enemiesToSpawn;


    private void Awake()
    {
        // Safety checks
        if(walker == null || runner == null || heavy == null || harpy == null || fatty == null || spitter == null)
        {
            Debug.LogError("EnemySpawner missing enemies game objects!");
        } 
        if(spawnPositions.Count == 0)
        {
            Debug.LogError("EnemySpawner missing spawn positions!");
        }
        LoadData();
        startWaveEvent.Register(GenerateWave);
        deathEnemyEvent.Register(DeathEnemy);
    }

    private void OnDestroy()
    {
        startWaveEvent.Unregister(GenerateWave);
        deathEnemyEvent.Unregister(DeathEnemy);
    }

    public void GenerateWave(int newWave)
    {
        currentWave = newWave;
        // Select a randomly spawn point for this wave
        selectedSpawnPosition = SelectSpawnPosition();
        // Spawn enemies
        StartCoroutine(SpawnEnemiesCoroutines());
    }

    private Transform SelectSpawnPosition()
    {
        return spawnPositions[Random.Range(0, spawnPositions.Count)];
    }

    private int CalculateWaveEnemies()
    {
        // Set the base enemies to spawn based on current way
        enemiesToSpawn = baseEnemiesPerWave * currentWave;

        // Add difficulty modifier
        switch (dificultyMode)
        {
            case DificultyMode.Easy: return enemiesToSpawn;
            case DificultyMode.Medium: return (int)(enemiesToSpawn * 1.5f);
            case DificultyMode.Hard | DificultyMode.Infinite: return enemiesToSpawn * 2;
        }

        return enemiesToSpawn;
    }

    private EnemyType GetEnemyTypeRandomly()
    {
        int roll = Random.Range(0, 100);

        int specialRatio = Mathf.Clamp(currentWave * 2, 0, 25);

        if(roll < specialRatio)
        {
            return GetSpecialEnemyRandomly();
        }

        if (roll < 50) return EnemyType.Walker;
        if (roll < 80) return EnemyType.Runner;
        return EnemyType.Heavy;
    }

    private EnemyType GetSpecialEnemyRandomly()
    {
        int selectedIndex = Random.Range(0, 3);
        
        switch(selectedIndex)
        {
            case 0: return EnemyType.Harpy;
            case 1: return EnemyType.Fatty;
            default: return EnemyType.Spitter;
        }
    }

    private GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        switch(enemyType)
        {
            case EnemyType.Walker: return walker;
            case EnemyType.Runner: return runner;
            case EnemyType.Heavy: return heavy;
            case EnemyType.Harpy: return harpy;
            case EnemyType.Spitter: return spitter;
            case EnemyType.Fatty: return fatty;
            default: return walker;
        }
    }

    private void SpawnEnemy()
    {
        EnemyType enemyTypeToSpawn = GetEnemyTypeRandomly();
        GameObject enemy = Instantiate(GetEnemyPrefab(enemyTypeToSpawn), selectedSpawnPosition.position, Quaternion.identity);
        unitRecruitedEvent.Raise(enemy.GetComponent<BaseUnit>());
    }

    private IEnumerator SpawnEnemiesCoroutines()
    {
        int enemyCount = CalculateWaveEnemies();

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(Mathf.Lerp(0.5f, 0.2f, currentWave / 20f));
        }
    }

    private void LoadData()
    {
        // Load Difficulty
        if (PlayerPrefs.HasKey("dificultyMode"))
        {
            dificultyMode = (DificultyMode)PlayerPrefs.GetInt("dificultyMode");
        }
    }

    private void DeathEnemy(Null @null)
    {
        enemiesToSpawn--;
        if(enemiesToSpawn <= 0)
        {
            finishWaveEvent.Raise(null);
        }
    }
}
