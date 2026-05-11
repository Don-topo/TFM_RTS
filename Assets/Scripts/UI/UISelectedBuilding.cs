using TMPro;
using UnityEngine;

public class UISelectedBuilding : MonoBehaviour
{
    [SerializeField] private UIProductionBuildingSelected uIProductionBuilding;
    [SerializeField] private UIRecruitUnitBuilding uIRecruitUnitBuilding;
    [SerializeField] private UIBuildSelectedBuilding uIBuildSelectedBuilding;
    [SerializeField] private TextMeshProUGUI buildingNameText;

    public void Enable(BaseBuilding baseBuilding)
    {
        Disable();
        if (baseBuilding.IsConstructed)
        {
            if (baseBuilding is ProductionBuilding)
            {                
                uIProductionBuilding.Enable((ProductionBuilding)baseBuilding);
            }
            else if (baseBuilding is RecruitBuilding)
            {
                // Recruiting building
                uIRecruitUnitBuilding.Enable((RecruitBuilding)baseBuilding);
            }
        }
        else
        {
            uIBuildSelectedBuilding.Enable(baseBuilding);
        }
        

        buildingNameText.enabled = true;
        buildingNameText.SetText(baseBuilding.SO_building.Name);
    }

    public void Disable()
    {
        buildingNameText.enabled = false;
        uIProductionBuilding.Disable();
        uIRecruitUnitBuilding.Disable();
        uIBuildSelectedBuilding.Disable();
    }
}
