using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [Header("Option Menus")]
    [SerializeField] private GameObject mainOptionsGameObject;
    [SerializeField] private GameObject graphicsGameObject;
    [SerializeField] private GameObject audioGameObject;
    [SerializeField] private GameObject gameplayGameObject;

    [Header("Graphics")]
    [SerializeField] private TMP_Dropdown resolutionsDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private TMP_Dropdown qualitySettingsDropdown;

    [Header("Sound")]
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] private Slider effectsVolumeSlider;
    [SerializeField] private Slider generalVolumeSlider;
    [SerializeField] private Slider voicesVolumeSlider;

    [Header("Gameplay")]
    [SerializeField] private Slider cameraMovementSlider;
    [SerializeField] private Slider mouseSensitivitySlider;
    // TODO Multi language?¿?

    private Resolution[] resolutions;

    private void Start()
    {
        DisableAllCanvas();
        PrepareResolutionDropdown();
        LoadSettings();
    }


    // Save options system using PlayerPrefs
    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("resolution"))
        {
            resolutionsDropdown.value = PlayerPrefs.GetInt("resolution");
            SetResolution(resolutionsDropdown.value);
        }

        if (PlayerPrefs.HasKey("fullScreen"))
        {            
            SetScreenMode(PlayerPrefs.GetInt("fullScreen") == 1);
        }

        //if(PlayerPrefs.HasKey())
    }

    public void SaveSettings()
    {
        // Graphics
        PlayerPrefs.SetInt("resolution", resolutionsDropdown.value);
        PlayerPrefs.SetInt("fullScreen", fullScreenToggle.isOn ? 1 : 0);
        //PlayerPrefs.SetInt("graphicsQuality", )

        // Audio
        PlayerPrefs.SetFloat("generalVolume", generalVolumeSlider.value);
        PlayerPrefs.SetFloat("voicesVolume", voicesVolumeSlider.value);
        PlayerPrefs.SetFloat("effectsVolume", effectsVolumeSlider.value);

        // Gameplay
        PlayerPrefs.SetFloat("mouseSensibility", mouseSensitivitySlider.value);
        PlayerPrefs.SetFloat("cameraMovementSpeed", cameraMovementSlider.value);

        //PlayerPrefs.SetInt("")
        PlayerPrefs.Save();
    }

    private void PrepareResolutionDropdown()
    {
        resolutions = Screen.resolutions;
        resolutionsDropdown.ClearOptions();
        for(int i = 0; i < resolutions.Length; i++)
        {

        }
    }

    // Options Setters
    // Graphics
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.width, Screen.fullScreen);
    }

    public void SetScreenMode(bool screenMode)
    {
        Screen.fullScreen = screenMode;
    }

    public void SetQualitySetting(int qualitySettingsIndex)
    {
        QualitySettings.SetQualityLevel(qualitySettingsIndex);
    }

    // Sound
    public void SetEffectsVolume(float volume)
    {                
        audioMixer.SetFloat("EffectsVolume", Mathf.Log10(volume) * 20);
    }
    
    public void SetVoicesVolume(float volume)
    {
        audioMixer.SetFloat("VoicesVolume", Mathf.Log10(volume) * 20);
    }

    public void SetGeneralVolume(float volume)
    {
        audioMixer.SetFloat("GeneralVolume", Mathf.Log10(volume) * 20);
    }

    // Gameplay
    public void SetCameraSpeed(float speed)
    {

    }

    public void SetMouseSensibility(float sensibility)
    {

    }

    // Navigations between options canvas
    private void DisableAllCanvas()
    {
        mainOptionsGameObject.SetActive(false);
        graphicsGameObject.SetActive(false);
        audioGameObject.SetActive(false);
        gameplayGameObject.SetActive(false);
    }

    public void GoToOptions()
    {
        mainOptionsGameObject.SetActive(true);
        graphicsGameObject.SetActive(false);
        audioGameObject.SetActive(false);
        gameplayGameObject.SetActive(false);
    }

    public void GoToGraphicsOptions()
    {
        mainOptionsGameObject.SetActive(false);
        graphicsGameObject.SetActive(true);
        audioGameObject.SetActive(false);
        gameplayGameObject.SetActive(false);
    }

    public void GoToSoundOptions()
    {
        mainOptionsGameObject.SetActive(false);
        graphicsGameObject.SetActive(false);
        audioGameObject.SetActive(true);
        gameplayGameObject.SetActive(false);
    }

    public void GoToGameplayOptions()
    {
        mainOptionsGameObject.SetActive(false);
        graphicsGameObject.SetActive(false);
        audioGameObject.SetActive(false);
        gameplayGameObject.SetActive(true);
    }
}