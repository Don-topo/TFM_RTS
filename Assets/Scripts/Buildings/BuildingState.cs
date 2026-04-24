using UnityEngine;

public struct BuildingState
{
   public enum State
    {
        Construction,
        Destroyed,
        Repairing
    }

    [field: SerializeField] public float StartConstruction {  get; private set; }
    [field: SerializeField] public State CurrentState { get; private set; }
    [field: SerializeField] public float BuildingProgress { get; private set; }

    public BuildingState(float startConstruction, State state, float buildingProgress)
    {
        StartConstruction = startConstruction;
        CurrentState = state;
        BuildingProgress = buildingProgress;
    }
}
