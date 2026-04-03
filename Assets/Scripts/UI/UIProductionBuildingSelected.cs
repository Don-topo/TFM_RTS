using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UIProductionBuildingSelected : MonoBehaviour
{
    [SerializeField] private UIProgressbar UIProgressbar;
    [SerializeField] private TextMeshProUGUI produceNumberText;
    [SerializeField] private Image resourceImage;

    private SO_Resource resourceToProduce;
    private ProductionBuilding selectedBuilding;

    private void Update()
    {
        if(selectedBuilding != null)
        {
            float startTime = selectedBuilding.StartTime;
            float currentTime = Time.time;
            float finishTime = selectedBuilding.resource.ObtainingTime;
            UIProgressbar.UpdateProgress(Mathf.Clamp01((Time.time - startTime) / (finishTime)));
        }    
    }

    public void Enable(ProductionBuilding selectedBuild)
    {
        gameObject.SetActive(true);
        selectedBuilding = selectedBuild;
        resourceImage.sprite = selectedBuilding.resource.Icon;
        produceNumberText.SetText(selectedBuilding.resource.ObtainedAmount.ToString());
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
