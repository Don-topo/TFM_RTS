using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitBaseInfo : MonoBehaviour
{
    [Header("Basic Info")]
    [SerializeField] private Image unitImage;
    [SerializeField] private TextMeshProUGUI heathText;
    [Header("Events")]
    [SerializeField] private UpdateHealthEvent healthEvent;

    private CommonActions action;

    private void Awake()
    {
        healthEvent.Register(UpdateHealth);
    }

    private void OnDestroy()
    {
        healthEvent.Unregister(UpdateHealth);
    }

    public void Disable()
    {
        // Fix to not found error when game stops
        if(gameObject != null)
        {
            gameObject.SetActive(false);
        }       
    }

    public void Enable(CommonActions newAction)

    {
        action = newAction;
        gameObject.SetActive(true);
        unitImage.sprite = action.SO_BaseUnit.Icon;
        UpdateData(action);        
    }

    private void UpdateData(CommonActions unit)
    {
        // Get percentage health
        float healthPercentage = (float)unit.CurrentHealth / (float)unit.MaxHealth;
        Color targetColor;
        // Lerp between red, yellow and green
        if (healthPercentage > 0.5f)
        {
            targetColor = Color.Lerp(
                Color.yellow,
                Color.green,
                (healthPercentage - 0.5f) * 2f
            );
        }
        else
        {
            targetColor = Color.Lerp(
                Color.red,
                Color.yellow,
                healthPercentage * 2f
            );
        }

        heathText.color = targetColor;
        heathText.SetText(unit.CurrentHealth.ToString() + " / " + unit.MaxHealth.ToString());
    }

    private void UpdateHealth(CommonActions unit)
    {
        if(unit == action)
        {
            UpdateData(unit);
        }        
    }
}
