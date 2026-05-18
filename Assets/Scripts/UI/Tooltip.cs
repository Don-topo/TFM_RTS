using TMPro;
using UnityEngine;


public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tooltipText;
    [field: SerializeField][Range(0, 1)] public float ShowWaitTime { get; private set; } = 0.5f;
    [SerializeField] private int offset = 50;
    [field: SerializeField] public RectTransform RectTransform { get; private set; }

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void SetTooltipText(string text)
    {
        tooltipText.SetText(text);
    }

    public void ShowTooltip()
    {
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}
