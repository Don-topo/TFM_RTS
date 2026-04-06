using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIRecruitUnitBuilding : MonoBehaviour
{
    [SerializeField] private List<UIRecruitButton> recruitButtons = new List<UIRecruitButton>();
    [SerializeField] private UIProgressbar progressbar;
    [SerializeField] private UpdateRecruitQueueEvent queueEvent;    

    private RecruitBuilding productionBuilding;
    private Coroutine coroutine;

    public void Enable(RecruitBuilding productionBuilding)
    {
        if(productionBuilding != null)
        {
            productionBuilding.queueEvent.Unregister(UpdateQueue);
        }
        gameObject.SetActive(true);
        this.productionBuilding = productionBuilding;
        progressbar.Enable();
        progressbar.UpdateProgress(0f);
        productionBuilding.queueEvent.Register(UpdateQueue);
        PrepareQueueButtons();

        coroutine = StartCoroutine(UpdateProgressBar());
    }

    public void Disable()
    {
        if(productionBuilding != null)
        {
            productionBuilding.queueEvent.Unregister(UpdateQueue);
        }
        gameObject.SetActive(false);
        productionBuilding = null;
        coroutine = null;
    }

    private void UpdateQueue(List<SO_BaseUnit> units)
    {
        if(productionBuilding != null)
        {
            PrepareQueueButtons();
        }

        if(units.Count == 1 && coroutine == null)
        {
            coroutine = StartCoroutine(UpdateProgressBar());
        }

        if (units.Count == 0) progressbar.UpdateProgress(0);
    }

    private void PrepareQueueButtons()
    {
        int i = 0;
        foreach(SO_BaseUnit recruitUnit in productionBuilding.GetRecruitQueue())
        {
            int unitIndex = i;
            if(recruitUnit != null)
            {
                recruitButtons[i].Enable(productionBuilding.GetRecruitQueue()[unitIndex], () => productionBuilding.CancelRecruitUnit(unitIndex));
            }
            else
            {
                recruitButtons[i].Disable();
            }
            
            i++;
        }

        for(; i < productionBuilding.GetMaxQueueSize(); i++)
        {
            recruitButtons[i].Disable();
        }
    }

    private IEnumerator UpdateProgressBar()
    {
        while(productionBuilding != null && productionBuilding.GetRecruitQueue().Count > 0)
        {
            float recruitStartTime = productionBuilding.RecruitStartTime;
            float recruitEndTime = recruitStartTime + productionBuilding.SO_BaseUnit.GenerationTime;

            float recruitProgress = Mathf.Clamp01((Time.time - recruitStartTime) / (recruitEndTime - recruitStartTime));

            progressbar.UpdateProgress(recruitProgress);
            yield return null;
        }
        coroutine = null;
    }
}
