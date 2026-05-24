using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }
    [SerializeField] private GameObject pauseMenu;

    private static GameObject pauseMenuPrefab;

    private void Awake()
    {
        pauseMenuPrefab = pauseMenu;    
    }

    public static void Pause()
    {
        IsPaused = true;

        Time.timeScale = 0f;

        pauseMenuPrefab.SetActive(true);
    }

    public static void Resume()
    {
        IsPaused = false;

        Time.timeScale = 1f;

        pauseMenuPrefab.SetActive(false);
    }

    public static void ResumeTime()
    {
        IsPaused = false;

        Time.timeScale = 1f;
    }    
}
