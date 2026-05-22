using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private EnemySpawner enemySpawner;
    [SerializeField] private DayManager dayManager;

    [Header("Game Properties")]
    [Range(0, 1)][SerializeField] private float dificultyModifier;
    [SerializeField] private int totalDays = 25; 
    [SerializeField] private float timeBetweenWaves = 320f;

    [Header("Events")]
    [SerializeField] private BuildingDestroyedEvent buildDestroyedEvent;
    [SerializeField] private VictoryEvent victoryEvent;
    [SerializeField] private GameOverEvent gameOverEvent;

    private void Awake()
    {
        StartCoroutine(Spawn());
        buildDestroyedEvent.Register(ManageBuildingDestroyed);
    }

    private void OnDestroy()
    {
        buildDestroyedEvent.Unregister(ManageBuildingDestroyed);
        StopAllCoroutines();
    }

    private void LoadData()
    {

    }

    private IEnumerator Spawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(10);
            enemySpawner.GenerateWave();
        }
    }

    private void ManageBuildingDestroyed(BaseBuilding building)
    {
        if (building.gameObject.CompareTag("CommandPost"))
        {
            StopAllCoroutines();
            // Game Over
            gameOverEvent.Raise(null);
            
        }
    }

    private void Win()
    {
        StopAllCoroutines();
        victoryEvent.Raise(null);
    }

           
    // TODO UI info (Events)
}
