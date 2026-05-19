using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIWinMenu : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject fadeGameObject;

    private void Awake()
    {
        exitButton.onClick.AddListener(GoToMainMenu);
    }

    private void OnDestroy()
    {
        exitButton.onClick.RemoveAllListeners();
    }

    private void GoToMainMenu()
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
