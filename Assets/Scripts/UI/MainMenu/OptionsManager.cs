using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class OptionsManager : MonoBehaviour
{
    [Header("Main menu")]
    [SerializeField] private MainMenu mainMenuGameObject;

    [Header("Option Menus")]
    [SerializeField] private GameObject mainOptionsGameObject;
    [SerializeField] private GameObject graphicsGameObject;
    [SerializeField] private GameObject audioGameObject;
    [SerializeField] private GameObject gameplayGameObject;
    [SerializeField] private Button graphicsButton;
    [SerializeField] private Button gameplayButton;
    [SerializeField] private Button audioButton;
    [SerializeField] private Button optionsBackButton;

    [Header("Graphics")]
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private TMP_Dropdown qualitySettingsDropdown;
    [SerializeField] private Button graphicsBackButton;
    [SerializeField] private Button graphicsSaveButton;

    [Header("Sound")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private Slider generalVolumeSlider;
    [SerializeField] private Slider voicesVolumeSlider;
    [SerializeField] private Button audioBackButton;
    [SerializeField] private Button audioSaveButton;

    [Header("Gameplay")]
    [SerializeField] private Slider cameraMovementSlider;
    [SerializeField] private Slider mouseSensitivitySlider;
    [SerializeField] private Button gameplayBackButton;
    [SerializeField] private Button gameplaySaveButton;

    private Resolution[] resolutions;

    private void Start()
    {
        SetButtons();
        DisableAllCanvas();
        GoToOptions();
        PrepareResolutionDropdown();
        PrepareGraphicsQualityDropdown();
        LoadSettings();
    }

    private void OnDestroy()
    {
        ClearButtons();
    }

    private void SetButtons()
    {
        // Main Options
        graphicsButton.onClick.AddListener(() => GoToGraphicsOptions());
        audioButton.onClick.AddListener(() => GoToSoundOptions());
        gameplayButton.onClick.AddListener(() => GoToGameplayOptions());
        optionsBackButton.onClick.AddListener(() => GoToMainMenu());
        // Graphics
        graphicsBackButton.onClick.AddListener(() => GoToOptions());
        graphicsSaveButton.onClick.AddListener(() => SaveSettings());
        // Gameplay
        gameplayBackButton.onClick.AddListener(() => GoToOptions());
        gameplaySaveButton.onClick.AddListener(() => SaveSettings());
        // Audio
        audioBackButton.onClick.AddListener(() => GoToOptions());
        audioSaveButton.onClick.AddListener(() => SaveSettings());
    }

    private void ClearButtons()
    {
        // Main options
        graphicsButton.onClick.RemoveAllListeners();
        gameplayButton.onClick.RemoveAllListeners();
        audioButton.onClick.RemoveAllListeners();
        optionsBackButton.onClick.RemoveAllListeners();
        // Graphics
        graphicsBackButton.onClick.RemoveAllListeners();
        graphicsSaveButton.onClick.RemoveAllListeners();
        graphicsSaveButton.onClick.RemoveAllListeners();
        // Audio
        audioBackButton.onClick.RemoveAllListeners();
        audioSaveButton.onClick.RemoveAllListeners();
        audioSaveButton.onClick.RemoveAllListeners();
        // Gameplay
        gameplayBackButton.onClick.RemoveAllListeners();
        gameplaySaveButton.onClick.RemoveAllListeners();
        gameplaySaveButton.onClick.RemoveAllListeners();
    }

    // Save options system using PlayerPrefs
    private void LoadSettings()
    {
        // Graphics
        if (PlayerPrefs.HasKey("resolution"))
        {            
            SetResolution(PlayerPrefs.GetInt("resolution"));
        }

        if (PlayerPrefs.HasKey("fullScreen"))
        {            
            SetScreenMode(PlayerPrefs.GetInt("fullScreen") == 1);
        }

        if (PlayerPrefs.HasKey("graphicsQuality"))
        {
            SetQualitySetting(PlayerPrefs.GetInt("graphicsQuality"));
        }

        // Audio
        if (PlayerPrefs.HasKey("generalVolume"))
        {
            SetGeneralVolume(PlayerPrefs.GetFloat("generalVolume"));
        }

        if (PlayerPrefs.HasKey("voicesVolume"))
        {
            SetVoicesVolume(PlayerPrefs.GetFloat("voicesVolume"));
        }

        if (PlayerPrefs.HasKey("effectsVolume"))
        {
            SetEffectsVolume(PlayerPrefs.GetFloat("effectsVolume"));
        }

        // Gameplay
        if (PlayerPrefs.HasKey("mouseSensibility"))
        {
            SetMouseSensibility(PlayerPrefs.GetFloat("mouseSensibility"));
        }

        if (PlayerPrefs.HasKey("cameraMovementSpeed"))
        {
            SetCameraSpeed(PlayerPrefs.GetFloat("cameraMovementSpeed"));
        }
    }

    private void SaveSettings()
    {
        // Graphics
        PlayerPrefs.SetInt("resolution", resolutionsDropdown.value);
        PlayerPrefs.SetInt("fullScreen", fullScreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetInt("graphicsQuality", qualitySettingsDropdown.value);

        // Audio
        PlayerPrefs.SetFloat("generalVolume", generalVolumeSlider.value);
        PlayerPrefs.SetFloat("voicesVolume", voicesVolumeSlider.value);
        PlayerPrefs.SetFloat("effectsVolume", effectsVolumeSlider.value);

        // Gameplay
        PlayerPrefs.SetFloat("mouseSensibility", mouseSensitivitySlider.value);
        PlayerPrefs.SetFloat("cameraMovementSpeed", cameraMovementSlider.value);

        PlayerPrefs.Save();

        GoToOptions();
    }

    private void PrepareResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();
        int index = 0;
        List<string> options = new();
        for(int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                index = i;
            }
        }

        resolutionsDropdown.AddOptions(options);
        resolutionsDropdown.value = index;
        resolutionsDropdown.RefreshShownValue();
    }

    private void PrepareGraphicsQualityDropdown()
    {
        
    }

    // Options Setters
    // Graphics
    private void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        resolutionsDropdown.value = PlayerPrefs.GetInt("resolution");
        Screen.SetResolution(resolution.width, resolution.width, Screen.fullScreen);
    }

    private void SetScreenMode(bool screenMode)
    {
        fullScreenToggle.isOn = screenMode;
        Screen.fullScreen = screenMode;
    }

    private void SetQualitySetting(int qualitySettingsIndex)
    {
        qualitySettingsDropdown.value = qualitySettingsIndex;
        QualitySettings.SetQualityLevel(qualitySettingsIndex);        
    }

    // Sound
    private void SetEffectsVolume(float volume)
    {                
        audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
        effectsVolumeSlider.value = Mathf.Clamp01(volume);
    }

    private void SetVoicesVolume(float volume)
    {
        audioMixer.SetFloat("VoicesVolume", Mathf.Log10(volume) * 20);
        voicesVolumeSlider.value = Mathf.Clamp01(volume);
    }

    private void SetGeneralVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
        generalVolumeSlider.value = Mathf.Clamp01(volume);
    }

    // Gameplay
    private void SetCameraSpeed(float speed)
    {
        cameraMovementSlider.value = Mathf.Clamp01(speed);
    }

    private void SetMouseSensibility(float sensibility)
    {
        mouseSensitivitySlider.value = Mathf.Clamp01(sensibility);
    }

    // Navigations between options canvas
    private void DisableAllCanvas()
    {
        mainOptionsGameObject.SetActive(false);
        graphicsGameObject.SetActive(false);
        audioGameObject.SetActive(false);
        gameplayGameObject.SetActive(false);
    }

    private void GoToOptions()
    {
        mainOptionsGameObject.SetActive(true);
        graphicsGameObject.SetActive(false);
        audioGameObject.SetActive(false);
        gameplayGameObject.SetActive(false);
    }

    private void GoToGraphicsOptions()
    {
        mainOptionsGameObject.SetActive(false);
        graphicsGameObject.SetActive(true);
        audioGameObject.SetActive(false);
        gameplayGameObject.SetActive(false);
    }

    private void GoToSoundOptions()
    {
        mainOptionsGameObject.SetActive(false);
        graphicsGameObject.SetActive(false);
        audioGameObject.SetActive(true);
        gameplayGameObject.SetActive(false);
    }

    private void GoToGameplayOptions()
    {
        mainOptionsGameObject.SetActive(false);
        graphicsGameObject.SetActive(false);
        audioGameObject.SetActive(false);
        gameplayGameObject.SetActive(true);
    }

    private void GoToMainMenu()
    {
        mainMenuGameObject.SetMainMenu();
    }
}