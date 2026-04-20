using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject optionsGameObject;
    [SerializeField] private GameObject difficultySelectorGameObject;
    [SerializeField] private GameObject mainMenuGameObject;
    [Header("Buttons")]
    [SerializeField] private Button startGameButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button exitButton;

    private void Start()
    {
        SetButtons();
        SetMainMenu();
    }

    private void OnDestroy()
    {
        ClearButtons();
    }

    private void SetButtons()
    {
        startGameButton.onClick.AddListener(() => SetDifficultySelector());
        optionsButton.onClick.AddListener(() => SetOptions());
        exitButton.onClick.AddListener(() => ExitGame());
    }

    private void ClearButtons()
    {
        startGameButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
        exitButton.onClick.RemoveAllListeners();
    }

    public void SetMainMenu()
    {
        optionsGameObject.SetActive(false);
        difficultySelectorGameObject.SetActive(false);
        mainMenuGameObject.SetActive(true);
    }

    private void SetOptions()
    {
        optionsGameObject.SetActive(true);
        difficultySelectorGameObject.SetActive(false);
        mainMenuGameObject.SetActive(false);
    }

    private void SetDifficultySelector()
    {
        optionsGameObject.SetActive(false);
        difficultySelectorGameObject.SetActive(true);
        mainMenuGameObject.SetActive(false);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
