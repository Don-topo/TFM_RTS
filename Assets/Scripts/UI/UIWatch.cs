using Unity.VisualScripting;
using UnityEngine;

public class UIWatch : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private StartWaveEvent startWaveEvent;
    [SerializeField] private FinishWaveEvent finishWaveEvent;
    [Header("Required Components")]
    [SerializeField] private Animator animator;

    private void Awake()
    {
        startWaveEvent.Register(HideWatch);
        finishWaveEvent.Register(ShowWatch);
    }

    private void OnDestroy()
    {
        startWaveEvent.Unregister(HideWatch);
        finishWaveEvent.Unregister(ShowWatch);
    }

    private void ShowWatch(Null @null)
    {
        animator.SetTrigger("Show");
    }

    private void HideWatch(int wave)
    {
        animator.SetTrigger("Hide");
    }
}
