using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private DificultyMode dificultyMode;
    //[SerializeField] private int basic

    private int currentWave = 0;
    private bool activeWave = false;
    private int baseEnemiesPerWave = 10;
   

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
    }

    public void GenerateWave(int waveNumber, float dificultyModifier, Vector3 spawnPosition)
    {
        StartCoroutine(SpawnEnemiesCoroutines());
    }

    private Transform SelectSpawnPosition()
    {
        return spawnPositions[Random.Range(0, spawnPositions.Count)];
    }

    private int CalculateWaveEnemies()
    {
        int enemiesToSpawn = baseEnemiesPerWave * 2 + 5;

        switch (dificultyMode)
        {
            case DificultyMode.Easy: return enemiesToSpawn;
            case DificultyMode.Medium: return ((int)(enemiesToSpawn * 1.5f));
            case DificultyMode.Hard | DificultyMode.Infinite: return enemiesToSpawn * 2;
        }

        return enemiesToSpawn;
    }

    private bool IsSPecialEnemy(EnemyType enemy)
    {
        return enemy == EnemyType.Harpy || enemy == EnemyType.Fatty || enemy == EnemyType.Spitter;
    }

    private EnemyType GetEnemyRandomly()
    {
        int roll = Random.Range(0, 100);

        int sp = Mathf.Clamp(currentWave * 2, 0, 30);

        if(roll < sp)
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
        EnemyType enemyTypeToSpawn = GetEnemyRandomly();
        Instantiate(GetEnemyPrefab(enemyTypeToSpawn), SelectSpawnPosition().position, Quaternion.identity);
    }

    private IEnumerator SpawnEnemiesCoroutines()
    {
        int enemyCount = CalculateWaveEnemies();

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();

            //yield return new WaitForSeconds(0.5f); 
            yield return new WaitForSeconds(Mathf.Lerp(0.5f, 0.2f, currentWave / 20f));
        }

        currentWave++;
    }
}
