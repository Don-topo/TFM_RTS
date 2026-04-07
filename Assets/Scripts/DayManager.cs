using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DayManager : MonoBehaviour
{
    [Header("Skybox")]
    [SerializeField] private Gradient skyboxTint;
    [SerializeField] private AnimationCurve skyboxExposure;
    [SerializeField] private AnimationCurve atmosphereThickness;
    [SerializeField] private Material skyboxMaterial;

    [Header("Lights")]
    [SerializeField] private Light sun;
    [SerializeField] private Light moon;

    [Header("Time")]
    [Range(0, 1)][SerializeField] private float timeOfDay;
    [SerializeField] private float dayDurationInMinutes = 5f;

    [Header("Curves")]
    [SerializeField] private AnimationCurve sunIntensity;
    [SerializeField] private AnimationCurve moonIntensity;
    [SerializeField] private Gradient lightColor;

    [Header("Post Processing")]
    [SerializeField] private Volume globalVolume;
    [SerializeField] private ColorAdjustments colorAdjustments;

    private void Start()
    {
        RenderSettings.skybox = skyboxMaterial;

        if (globalVolume != null)
        {
            globalVolume.profile.TryGet(out colorAdjustments);
        }
    }

    private void Update()
    {
        float dayDurationInSeconds = dayDurationInMinutes * 60f;
        float speed = 1f / dayDurationInSeconds;

        timeOfDay += Time.deltaTime * speed;
        if (timeOfDay > 1f) timeOfDay = 0f;

        UpdateLighting();
    }

    private void UpdateLighting()
    {
        float sunInt = sunIntensity.Evaluate(timeOfDay);
        float moonInt = moonIntensity.Evaluate(timeOfDay);

        sun.intensity = sunInt;
        moon.intensity = moonInt;

        Color col = lightColor.Evaluate(timeOfDay);
        sun.color = col;

        float angle = timeOfDay * 360f - 90f;

        sun.transform.rotation = Quaternion.Euler(angle, 170, 0);
        moon.transform.rotation = Quaternion.Euler(angle + 180f, 170, 0);

        if (colorAdjustments != null)
        {
            float exposure = Mathf.Lerp(1.5f, 0.3f, sunInt);
            colorAdjustments.postExposure.value = exposure;
        }

        if (skyboxMaterial != null)
        {
            Color tint = skyboxTint.Evaluate(timeOfDay);
            float exposure = skyboxExposure.Evaluate(timeOfDay);
            float atmosphere = atmosphereThickness.Evaluate(timeOfDay);

            skyboxMaterial.SetColor("_SkyTint", tint);
            skyboxMaterial.SetFloat("_Exposure", exposure);
            skyboxMaterial.SetFloat("_AtmosphereThickness", atmosphere);
        }
    }
}
