using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIDefeatMenu : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button retryButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject fadeGameObject;

    private void Awake()
    {
        retryButton.onClick.AddListener(RetryGame);
        exitButton.onClick.AddListener(BackToMainMenu);
    }

    private void OnDestroy()
    {
        retryButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }

    private void RetryGame()
    {
        fadeGameObject.SetActive(true);
        StartCoroutine(nameof(FadeOut), "Game");
    }

    private void BackToMainMenu()
    {
        fadeGameObject.SetActive(true);
        StartCoroutine(nameof(FadeOut), "Menu");        
    }
    private IEnumerator FadeOut(string scene)
    {
        yield return new WaitForSeconds(1.9f);
        SceneManager.LoadScene(scene);
    }
}
