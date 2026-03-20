using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitBaseInfo : MonoBehaviour
{
    [SerializeField] private Image unitImage;
    [SerializeField] private TextMeshProUGUI heathText;

    private CommonActions action;

    public void Disable()
    {
       gameObject.SetActive(false);
    }

    public void Enable(CommonActions newAction)

    {
        action = newAction;
        gameObject.SetActive(true);
        unitImage.sprite = action.so_baseUnit.Icon;
        heathText.SetText(action.so_baseUnit.Health.ToString());
    }

    private void UpdateHealthText()
    {
        
    }
}
