using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RecruitBuilding : BaseBuilding
{
    [SerializeField] private ResourceEvent resourceEvent;
    [field: SerializeField] public UpdateRecruitQueueEvent queueEvent;
    [SerializeField] private RefreshUIEvent refreshEvent;
    [SerializeField] private GameObject recruitDestination;
    private List<SO_BaseUnit> RecruitUnitsQueue = new List<SO_BaseUnit>(RECRUIT_QUEUE_SIZE);
    private const int RECRUIT_QUEUE_SIZE = 5;
    private bool isRecruitDestinationSet = false;
    private GameObject recruitDestinationInstance;
    public float RecruitStartTime;

    public List<SO_BaseUnit> GetRecruitQueue() => RecruitUnitsQueue;
    public int GetMaxQueueSize() => RECRUIT_QUEUE_SIZE;

    public void SetRecruitDestination(Vector3 newPosition)
    {
        isRecruitDestinationSet = newPosition != null;
        if (isRecruitDestinationSet && recruitDestinationInstance != null)
        {
            recruitDestinationInstance.transform.position = newPosition;
            recruitDestinationInstance.SetActive(true);
        }               
    }

    protected override void Awake()
    {
        base.Awake();
        recruitDestinationInstance = Instantiate(recruitDestination);
        recruitDestinationInstance.SetActive(false);
        Deselect();        
    }

    public void RecruitUnit(SO_BaseUnit unitToRecruit)
    {
        // Check if the queue is full
        if (RecruitUnitsQueue.Count == RECRUIT_QUEUE_SIZE) return;

        // Spend resources
        ResourceOP resourceOP = new ResourceOP(unitToRecruit.Cost.SO_Food, unitToRecruit.Cost.Food, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(unitToRecruit.Cost.SO_Wood, -unitToRecruit.Cost.Wood, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(unitToRecruit.Cost.SO_Stone, -unitToRecruit.Cost.Stone, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(unitToRecruit.Cost.SO_Iron, -unitToRecruit.Cost.Iron, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(unitToRecruit.Cost.SO_Electricity, unitToRecruit.Cost.Electricity, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(unitToRecruit.Cost.SO_Population, unitToRecruit.Cost.Population, 0);
        resourceEvent.Raise(resourceOP);
        
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
        ResourceOP resourceOP = new ResourceOP(unitToRemove.Cost.SO_Food, -unitToRemove.Cost.Food, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(unitToRemove.Cost.SO_Wood, unitToRemove.Cost.Wood, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(unitToRemove.Cost.SO_Stone, unitToRemove.Cost.Stone, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(unitToRemove.Cost.SO_Iron, unitToRemove.Cost.Iron, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(unitToRemove.Cost.SO_Electricity, -unitToRemove.Cost.Electricity, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(unitToRemove.Cost.SO_Population, -unitToRemove.Cost.Population, 0);
        resourceEvent.Raise(resourceOP);
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
        if (isRecruitDestinationSet)
        {
            recruitDestinationInstance.SetActive(true);
        }
    }

    public override void Deselect()
    {
        base.Deselect();
        if (isRecruitDestinationSet)
        {
            recruitDestinationInstance.SetActive(false);
        }
    }

    private void SetRecruitDestination(GameObject recruitedUnit)
    {
        if (isRecruitDestinationSet)
        {
            if(recruitedUnit.TryGetComponent(out IMoveable moveable))
            {
                moveable.Move(recruitDestinationInstance.transform.position);
            }
        }
    }

    private IEnumerator RecruitUnits()
    {
        while(RecruitUnitsQueue.Count > 0)
        {
            // Refresh all UI
            refreshEvent.Raise(true);
            // Start by the first unit in the queue
            SO_BaseUnit unitToRecruit = RecruitUnitsQueue[0];
            RecruitStartTime = Time.time;
            // Send Queue update event
            queueEvent.Raise(RecruitUnitsQueue);
            // Wait for the recruiting time
            yield return new WaitForSeconds(unitToRecruit.GenerationTime);
            // Instantiate the recruited unit
            GameObject recruitedUnit = Instantiate(unitToRecruit.UnitPrefab, transform.position, Quaternion.identity);
            // If the recruit destination is set move to them
            SetRecruitDestination(recruitedUnit);
            // Remove the recruited unit
            RecruitUnitsQueue.RemoveAt(0);
            // Notify the update of the queue
            queueEvent.Raise(RecruitUnitsQueue);
            // Refresh all UI
            refreshEvent.Raise(true);
        }
    }
}
