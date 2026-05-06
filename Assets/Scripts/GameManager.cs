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

    [Header("Events")]
    [SerializeField] private BuildingDestroyedEvent buildDestroyedEvent;

    private int currentDay = 0;

    private void Awake()
    {
        StartCoroutine(Spawn());
        buildDestroyedEvent.Register(ManageBuildingDestroyed);
    }

    private void OnDestroy()
    {
        buildDestroyedEvent?.Unregister(ManageBuildingDestroyed);
    }

    private IEnumerator Spawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(10);
            enemySpawner.GenerateWave(0, DificultyMode.Easy, Vector3.zero);
        }
    }

    private void ManageBuildingDestroyed(BaseBuilding building)
    {
        if (building.gameObject.CompareTag("CommandPost"))
        {
            // Game Over
        }
    }

    // TODO get Building destroyed event and check if is a comandPost

    // TODO Win condition
    // TODO Lose condition
    // TODO Generate enemy waves
    // TODO UI info (Events)
}
