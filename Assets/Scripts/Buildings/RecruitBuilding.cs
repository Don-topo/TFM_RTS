using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RecruitBuilding : BaseBuilding
{
    [SerializeField] private ResourceEvent resourceEvent;
    private List<BaseUnit> RecruitUnitsQueue = new List<BaseUnit>(RECRUIT_QUEUE_SIZE);
    private const int RECRUIT_QUEUE_SIZE = 5;

    public void RecruitUnit()
    {

    }

    public void CancelRecruitUnit(int queueIndex)
    {
        // Safety check
        if (queueIndex < 0 || queueIndex > RECRUIT_QUEUE_SIZE) return;

        // Get the unit from the queue
        BaseUnit unitToRemove = RecruitUnitsQueue[queueIndex];
        // Return Recruiting resources
        resourceEvent.Raise(unitToRemove.SO_BaseUnit.Cost.SO_Food);
        resourceEvent.Raise(unitToRemove.SO_BaseUnit.Cost.SO_Wood);
        resourceEvent.Raise(unitToRemove.SO_BaseUnit.Cost.SO_Iron);
        resourceEvent.Raise(unitToRemove.SO_BaseUnit.Cost.SO_Stone);
        resourceEvent.Raise(unitToRemove.SO_BaseUnit.Cost.SO_Electricity);
        // Delete unit from recruit queue
        RecruitUnitsQueue.RemoveAt(queueIndex);

        // Check if the recruit queue is empty
        if(RecruitUnitsQueue.Count == 0)
        {

        }
    }

    private IEnumerator RecruitUnits()
    {
        while(RecruitUnitsQueue.Count > 0)
        {
            // Start by the first unit in the queue
            BaseUnit unitToRecruit = RecruitUnitsQueue[0];
            float RecruitStartTime = Time.time;
            // Send Queue update event
            // If there is not enought resources wait until 
            // Send increase population event

            // Wait for the recruiting time
            yield return new WaitForSeconds(unitToRecruit.SO_BaseUnit.GenerationTime);

            // Remove the recruited unit
            RecruitUnitsQueue.RemoveAt(0);

            // Notify the update of the queue
        }
    }
}
