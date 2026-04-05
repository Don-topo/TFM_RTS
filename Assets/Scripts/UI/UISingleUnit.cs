using TMPro;
using UnityEngine;

public class UISingleUnit : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitNameText;

    CommonActions unitSelected;

    public void Enable(CommonActions unitSelected)
    {
        gameObject.SetActive(true);
        this.unitSelected = unitSelected;
        unitNameText.SetText(unitSelected.SO_BaseUnit.Name);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
