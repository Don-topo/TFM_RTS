using UnityEngine;

public interface IMoveable
{
    void Move(Transform transform);
    void Move(Vector3 position);
    void StopMove();
}
