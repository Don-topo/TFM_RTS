using TMPro;
using UnityEngine;

public class UISelectedBuilding : MonoBehaviour
{
    [SerializeField] private UIProductionBuildingSelected uIProductionBuilding;
    [SerializeField] private UIRecruitUnitBuilding uIRecruitUnitBuilding;
    [SerializeField] private TextMeshProUGUI buildingNameText;

    public void Enable(BaseBuilding baseBuilding)
    {
        if(baseBuilding is ProductionBuilding)
        {
            uIProductionBuilding.Enable((ProductionBuilding)baseBuilding);
            buildingNameText.enabled = true;
            buildingNameText.SetText(baseBuilding.SO_BaseUnit.name);
        }
        else
        {
            // Recruiting building
            uIRecruitUnitBuilding.Enable((RecruitBuilding)baseBuilding);
            buildingNameText.enabled = true;
            buildingNameText.SetText(baseBuilding.SO_BaseUnit.name);
        }
    }

    public void Disable()
    {
        buildingNameText.enabled = false;
        uIProductionBuilding.Disable();
        uIRecruitUnitBuilding.Disable();
    }
}
