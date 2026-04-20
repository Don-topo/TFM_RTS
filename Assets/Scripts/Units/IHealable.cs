using UnityEngine;

public interface IHealable
{
    public int CurrentHealth { get; }
    public int MaxHealth { get; }
    public Transform TargetPosition { get; }
    public void Heal(int amount);
}