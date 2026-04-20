using UnityEngine;

public interface IHealer {
    public Transform Transform { get; }
    public void Heal(IHealable unit);
    public void Heal(Vector3 attackPosition);
}