using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIProductionBuildingSelected : MonoBehaviour
{
    [Header("Progress Bar")]
    [SerializeField] private UIProgressbar uiProgressbar;
    [SerializeField] private GameObject progressBarBackground;
    [Header("Info Values")]
    [SerializeField] private TextMeshProUGUI produceNumberText;
    [SerializeField] private Image resourceImage;

    private ProductionBuilding selectedBuilding;

    private void Update()
    {
        if(selectedBuilding != null && !selectedBuilding.Resource.ProducesOnlyOneTime)
        {
            float startTime = selectedBuilding.StartTime;
            float currentTime = Time.time;
            float finishTime = selectedBuilding.Resource.ObtainingTime;
            uiProgressbar.UpdateProgress(Mathf.Clamp01((Time.time - startTime) / (finishTime)));
        }  
    }

    public void Enable(ProductionBuilding selectedBuild)
    {
        gameObject.SetActive(true);        
        selectedBuilding = selectedBuild;
        resourceImage.sprite = selectedBuilding.Resource.Icon;
        produceNumberText.SetText(selectedBuilding.Resource.MaxAmount.ToString());
        if (!selectedBuilding.Resource.ProducesOnlyOneTime)
        {
            uiProgressbar.Enable();
            progressBarBackground.SetActive(true);
            produceNumberText.SetText(selectedBuilding.Resource.ObtainedAmount.ToString());
        }
        else
        {
            uiProgressbar.Disable();
            progressBarBackground.SetActive(false);
        }
    }

    public void Disable()
    {
        uiProgressbar.Disable();
        gameObject.SetActive(false);        
    }
}
