using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRecruitUnitBuilding : MonoBehaviour
{
    [SerializeField] private UIRecruitButton[] recruitButtons;
    [SerializeField] private UIProgressbar progressbar;
    [SerializeField] private UpdateRecruitQueueEvent queueEvent;

    private ProductionBuilding productionBuilding;

    public void Enable(ProductionBuilding productionBuilding)
    {
        this.productionBuilding = productionBuilding;
        
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }

    private void UpdateQueue(List<BaseUnit> units)
    {

    }
}
