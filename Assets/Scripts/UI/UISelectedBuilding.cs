using TMPro;
using UnityEngine;

public class UISelectedBuilding : MonoBehaviour
{
    [SerializeField] private UIProductionBuildingSelected uIProductionBuilding;
    [SerializeField] private TextMeshProUGUI buildingNameText;

    public void Enable(BaseBuilding baseBuilding)
    {
        if(baseBuilding is ProductionBuilding)
        {
            uIProductionBuilding.Enable((ProductionBuilding)baseBuilding);
            buildingNameText.enabled = true;
            buildingNameText.SetText(baseBuilding.so_baseUnit.name);
        }
    }

    public void Disable()
    {
        buildingNameText.enabled = false;
        uIProductionBuilding.Disable();
    }
}
