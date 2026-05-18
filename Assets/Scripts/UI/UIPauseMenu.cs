using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour
{
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button exitButton;

    private void Awake()
    {
        exitButton.onClick.AddListener(GoToMainMenu);
        resumeButton.onClick.AddListener(GoToGame);
    }

    private void OnDestroy()
    {
        exitButton.onClick.RemoveAllListeners();
        resumeButton.onClick.RemoveAllListeners();
    }



    private void GoToGame()
    {

    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
