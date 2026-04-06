using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RecruitBuilding : BaseBuilding
{
    [SerializeField] private ResourceEvent resourceEvent;
    [field: SerializeField] public UpdateRecruitQueueEvent queueEvent;
    private List<SO_BaseUnit> RecruitUnitsQueue = new List<SO_BaseUnit>(RECRUIT_QUEUE_SIZE);
    private const int RECRUIT_QUEUE_SIZE = 5;
    public float RecruitStartTime;

    public List<SO_BaseUnit> GetRecruitQueue() => RecruitUnitsQueue;
    public int GetMaxQueueSize() => RECRUIT_QUEUE_SIZE;

    public void RecruitUnit(SO_BaseUnit unitToRecruit)
    {
        // Check if the queue is full
        if (RecruitUnitsQueue.Count == RECRUIT_QUEUE_SIZE) return;

        // Spend resources
        resourceEvent.Raise(unitToRecruit.Cost.SO_Food);
        resourceEvent.Raise(unitToRecruit.Cost.SO_Wood);
        resourceEvent.Raise(unitToRecruit.Cost.SO_Stone);
        resourceEvent.Raise(unitToRecruit.Cost.SO_Iron);
        resourceEvent.Raise(unitToRecruit.Cost.SO_Electricity);
        resourceEvent.Raise(unitToRecruit.Cost.SO_Population);
        
        // Add unit to the recruit queue
        RecruitUnitsQueue.Add(unitToRecruit);
        // If the queue is empty start recruiting
        if(RecruitUnitsQueue.Count == 1)
        {
            StartCoroutine(RecruitUnits());
        }
        else
        {
            // Else inform UI about the update of the queue
            queueEvent.Raise(RecruitUnitsQueue);
        }
    }

    public void CancelRecruitUnit(int queueIndex)
    {
        // Safety check
        if (queueIndex < 0 || queueIndex > RECRUIT_QUEUE_SIZE) return;

        // Get the unit from the queue
        SO_BaseUnit unitToRemove = RecruitUnitsQueue[queueIndex];
        // Return Recruiting resources
        resourceEvent.Raise(unitToRemove.Cost.SO_Food);
        resourceEvent.Raise(unitToRemove.Cost.SO_Wood);
        resourceEvent.Raise(unitToRemove.Cost.SO_Iron);
        resourceEvent.Raise(unitToRemove.Cost.SO_Stone);
        resourceEvent.Raise(unitToRemove.Cost.SO_Electricity);
        resourceEvent.Raise(unitToRemove.Cost.SO_Population);
        // Delete unit from recruit queue
        RecruitUnitsQueue.RemoveAt(queueIndex);

        if(queueIndex == 0)
        {
            // If the canceled unit is the one that currently is recruiting stop coroutine
            StopAllCoroutines();
            // Check if there is more units to recruit in the queue
            if(RecruitUnitsQueue.Count > 0)
            {
                StartCoroutine(RecruitUnits());
                return;
            }            
        }
        
        queueEvent.Raise(RecruitUnitsQueue);
        
    }

    public override void Select()
    {
        base.Select();
    }

    public override void Deselect()
    {
        base.Deselect();
    }

    private IEnumerator RecruitUnits()
    {
        while(RecruitUnitsQueue.Count > 0)
        {
            // Start by the first unit in the queue
            SO_BaseUnit unitToRecruit = RecruitUnitsQueue[0];
            RecruitStartTime = Time.time;
            // Send Queue update event
            queueEvent.Raise(RecruitUnitsQueue);
            // Wait for the recruiting time
            yield return new WaitForSeconds(unitToRecruit.GenerationTime);
            // Instantiate the recruited unit
            GameObject recruitedUnit = Instantiate(unitToRecruit.UnitPrefab, transform.position, Quaternion.identity);
            // Remove the recruited unit
            RecruitUnitsQueue.RemoveAt(0);
            // Notify the update of the queue
            queueEvent.Raise(RecruitUnitsQueue);
        }
    }
}
