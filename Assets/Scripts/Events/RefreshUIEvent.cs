using UnityEngine;

[CreateAssetMenu(fileName = "RefreshUIEvent", menuName = "Events/Refresh UI Event")]
public class RefreshUIEvent : GameEvent<bool>
{
    bool refresh;
}
