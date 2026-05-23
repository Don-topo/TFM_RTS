using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UIBuildSelectedBuilding : MonoBehaviour
{
    [SerializeField] private UIProgressbar progressBar;
    [SerializeField] private TextMeshProUGUI buildName;
    public void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Enable(BaseBuilding baseBuilding)
    {
        gameObject.SetActive(true);
        StopAllCoroutines();
        buildName.SetText(baseBuilding.SO_building.Name);
        StartCoroutine(UpdateProgressBar(baseBuilding));
    }

    private IEnumerator UpdateProgressBar(BaseBuilding baseBuilding)
    {
        float maxHealth = baseBuilding.SO_building.Health;

        while (baseBuilding.CurrentHealth < maxHealth)
        {
            float progress =
                baseBuilding.CurrentHealth / maxHealth;

            progressBar.UpdateProgress(progress);

            yield return null;
        }

        progressBar.UpdateProgress(1f);
        /*float startTime = baseBuilding.StartTime;
        float endTime =  startTime + baseBuilding.SO_building.GenerationTime;
        progressBar.UpdateProgress(Mathf.Clamp01((Time.time - startTime) / (endTime - startTime)));
        yield return null;*/
    }
}
