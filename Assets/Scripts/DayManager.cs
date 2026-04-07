using UnityEngine;
using UnityEngine.Rendering;

public class DayManager : MonoBehaviour
{
    public Material skyboxMaterial;

    [Header("Skybox")]
    public Gradient skyboxTint;
    public AnimationCurve skyboxExposure;
    public AnimationCurve atmosphereThickness;

    [Header("Lights")]
    public Light sun;
    public Light moon;

    [Header("Time")]
    [Range(0, 1)] public float timeOfDay;
    public float dayDurationInMinutes = 5f;

    [Header("Curves")]
    public AnimationCurve sunIntensity;
    public AnimationCurve moonIntensity;
    public Gradient lightColor;

    [Header("Post Processing")]
    public Volume globalVolume;
    private UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;

    void Start()
    {
        RenderSettings.skybox = skyboxMaterial;

        if (globalVolume != null)
        {
            globalVolume.profile.TryGet(out colorAdjustments);
        }
    }

    void Update()
    {
        float dayDurationInSeconds = dayDurationInMinutes * 60f;
        float speed = 1f / dayDurationInSeconds;

        timeOfDay += Time.deltaTime * speed;
        if (timeOfDay > 1f) timeOfDay = 0f;

        UpdateLighting();
    }

    void UpdateLighting()
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

        // Ajuste de exposición para RTS (clave)
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
