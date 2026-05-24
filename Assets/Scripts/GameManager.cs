using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class GameManager : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private UIProgressbar progressbar;
    [SerializeField] private TextMeshProUGUI remainingDaysText;
    [Header("Managers")]
    [SerializeField] private DayManager dayManager;

    [Header("Game Properties")]        
    [SerializeField] private float timeBetweenWaves = 320f;
    [SerializeField] private float timeBetweenDays;
    [SerializeField] private int easyTotalWaves = 25;
    [SerializeField] private int mediumTotalWaves = 25;
    [SerializeField] private int hardTotalWaves = 25;

    [Header("Events")]
    [SerializeField] private BuildingDestroyedEvent buildDestroyedEvent;
    [SerializeField] private VictoryEvent victoryEvent;
    [SerializeField] private GameOverEvent gameOverEvent;
    [SerializeField] private FinishWaveEvent finishWaveEvent;
    [SerializeField] private StartWaveEvent startWaveEvent;

    private DificultyMode difficultyModifier;
    private int currentWave;
    private float startTime;
    private int totalWaves = 25;

    private void Awake()
    {
        buildDestroyedEvent.Register(ManageBuildingDestroyed);
        finishWaveEvent.Register(WaveFinished);
        LoadData();

        currentWave = 1;
        SetWavesText();
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
                totalWaves = easyTotalWaves;
                timeBetweenWaves *= 1;
                break;
            case DificultyMode.Medium:
                totalWaves = mediumTotalWaves;
                timeBetweenWaves *= 0.75f;
                break;
            case DificultyMode.Hard:
                totalWaves = hardTotalWaves;
                timeBetweenWaves *= 0.5f;
                break;
            case DificultyMode.Infinite:
                totalWaves = 99999999;
                timeBetweenWaves *= 0.5f;
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
        if(currentWave > totalWaves)
        {
            Win();
            return;
        }
        startTime = Time.time;
        SetWavesText();
        StartCoroutine(nameof(FillWatch));
    }

    private void SetWavesText()
    {
        // Use text instead of settex to avoid breaking the animations
        if (difficultyModifier == DificultyMode.Infinite)
        {            
            remainingDaysText.text = "Survived Days " + currentWave.ToString();
        }
        else
        {
            remainingDaysText.text = "Remaining Days " + (totalWaves - currentWave + 1).ToString();
        }

        remainingDaysText.ForceMeshUpdate();
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
}
