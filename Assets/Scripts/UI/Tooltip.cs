using TMPro;
using UnityEngine;

[RequireComponent (typeof(RectTransform))]
public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tooltipText;
    [field: SerializeField][Range(0, 1)] public float ShowWaitTime { get; private set; } = 0.5f;
    [SerializeField] private int offset = 50;

    public RectTransform RectTransform { get; private set; }

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
    }

    public void SetTooltipText(string text)
    {
        tooltipText.SetText(text);
        // Change size
        Vector2 size = tooltipText.GetPreferredValues();
        RectTransform.sizeDelta = new Vector2(size.x + offset, size.y + (offset / 2)); 
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
