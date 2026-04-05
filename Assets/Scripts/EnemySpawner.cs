using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject basicMeleeEnemy;
    [SerializeField] private GameObject basicDistanceEnemy;
    [SerializeField] private GameObject eliteEnemy;

    private void Awake()
    {
        if(basicMeleeEnemy != null || basicDistanceEnemy || eliteEnemy)
        {
            Debug.LogError("EnemySpawner missing parameters!");
        } 
    }

    public void GenerateWave(int waveNumber, float dificultyModifier, Vector3 spawnPosition)
    {

    }
}
