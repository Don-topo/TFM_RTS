using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static bool IsPaused { get; private set; }
    [SerializeField] private static GameObject pauseMenu;


    void Update()
    {
        
    }

    public static void Pause()
    {
        IsPaused = true;

        Time.timeScale = 0f;

        pauseMenu.SetActive(true);

        AudioListener.pause = true;
    }

    public static void Resume()
    {
        IsPaused = false;

        Time.timeScale = 1f;

        pauseMenu.SetActive(false);

        AudioListener.pause = false;
    }

    
}
