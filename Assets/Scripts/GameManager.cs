using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private UIProgressbar progressbar;
    [SerializeField] private Button pauseButton;
    [Header("Managers")]
    [SerializeField] private DayManager dayManager;

    [Header("Game Properties")]    
    [SerializeField] private int totaWaves = 25; 
    [SerializeField] private float timeBetweenWaves = 320f;
    [SerializeField] private float timeBetweenDays;

    [Header("Events")]
    [SerializeField] private BuildingDestroyedEvent buildDestroyedEvent;
    [SerializeField] private VictoryEvent victoryEvent;
    [SerializeField] private GameOverEvent gameOverEvent;
    [SerializeField] private FinishWaveEvent finishWaveEvent;
    [SerializeField] private StartWaveEvent startWaveEvent;

    private DificultyMode difficultyModifier;
    private int currentWave;
    private float startTime;

    private void Awake()
    {
        buildDestroyedEvent.Register(ManageBuildingDestroyed);
        finishWaveEvent.Register(WaveFinished);
        LoadData();

        currentWave = 1;
        StartCoroutine(nameof(FillWatch));
    }

    private void OnDestroy()
    {
        buildDestroyedEvent.Unregister(ManageBuildingDestroyed);
        finishWaveEvent.Unregister(WaveFinished);
        StopAllCoroutines();
    }

    private void LoadData()
    {
        // Load Difficulty
        if (PlayerPrefs.HasKey("dificultyMode"))
        {
            difficultyModifier = (DificultyMode)PlayerPrefs.GetInt("dificultyMode");
        }

        switch (difficultyModifier)
        {
            case DificultyMode.Easy:
                break;
            case DificultyMode.Medium:
                break;
            case DificultyMode.Hard:
                break;
            case DificultyMode.Infinite:
                totaWaves = 99999999;
                break;
            default:
                break;
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

    private void WaveFinished(Null @null)
    {
        currentWave++;
        // Check for victory
        if(currentWave > totaWaves)
        {
            Win();
            return;
        }
        startTime = Time.time;
        StartCoroutine(nameof(FillWatch));
    }

    private IEnumerator FillWatch()
    {        
        while(Time.time < startTime + timeBetweenWaves)
        {
            yield return new WaitForSeconds(1f);
            // Update watch value
            float progress = Mathf.Clamp01(Time.time / (startTime + timeBetweenWaves));
            // Update watch
            progressbar.UpdateProgress(progress);
            // Calculate and update progressbar color
            float normalizeTime = Mathf.SmoothStep(0, 1, 1f - progress);
            Color currentColor = Color.Lerp(Color.red, Color.green, normalizeTime);
            progressbar.SetColor(currentColor);
        }
        // Start Wave
        startWaveEvent.Raise(currentWave);
    }

    // TODO UI info (Events)
    // Day x Animation
    // Horde is Comming
    // Horde Finish
}
