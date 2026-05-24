using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPauseMenu : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject fadeGameObject;

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
        PauseManager.Resume();
    }

    private void GoToMainMenu()
    {
        fadeGameObject.SetActive(true);        
        StartCoroutine(nameof(FadeOut), "Menu");
        PauseManager.ResumeTime();
    }

    private IEnumerator FadeOut(string scene)
    {
        yield return new WaitForSeconds(1.9f);
        SceneManager.LoadScene("Menu");
    }
}
