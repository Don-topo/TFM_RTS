using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DifficultySelector : MonoBehaviour
{
    [Header("MainMenu")]
    [SerializeField] private MainMenu mainMenu;

    [Header("Buttons")]
    [SerializeField] private Button easyButton;
    [SerializeField] private Button mediumButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button infiniteButton;
    [SerializeField] private Button backButton;

    private void Start()
    {
        SetButtons();
    }

    private void OnDestroy()
    {
        ClearButtons();
    }

    private void SetButtons()
    {
        easyButton.onClick.AddListener(() => StartGame(DificultyMode.Easy));
        mediumButton.onClick.AddListener(() => StartGame(DificultyMode.Medium));
        hardButton.onClick.AddListener(() => StartGame(DificultyMode.Hard));
        infiniteButton.onClick.AddListener(() => StartGame(DificultyMode.Infinite));
        backButton.onClick.AddListener(() => BackToMainMenu());
    }

    private void ClearButtons()
    {
        easyButton.onClick.RemoveAllListeners();
        mediumButton.onClick.RemoveAllListeners();
        hardButton.onClick.RemoveAllListeners();
        infiniteButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }

    private void StartGame(DificultyMode dificulty)
    {
        PlayerPrefs.SetInt("dificultyMode", (int)dificulty);
        PlayerPrefs.Save();
        SceneManager.LoadScene("SampleScene");
    }

    private void BackToMainMenu()
    {
        mainMenu.SetMainMenu();
    }
}
