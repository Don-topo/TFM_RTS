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

    private int currentDay = 0;

    private void Awake()
    {
        StartCoroutine(Spawn());
    }

    private IEnumerator Spawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(10);
            enemySpawner.GenerateWave(0, dificultyModifier, Vector3.zero);
        }
    }

    // TODO get Building destroyed event and check if is a comandPost

    // TODO Win condition
    // TODO Lose condition
    // TODO Generate enemy waves
    // TODO UI info (Events)
}
