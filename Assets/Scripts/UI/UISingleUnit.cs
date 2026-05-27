using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class UISingleUnit : MonoBehaviour
{
    [Header("Required Components")]
    [SerializeField] private TextMeshProUGUI unitNameText;
    [SerializeField] private TextMeshProUGUI attackDamageText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    [SerializeField] private TextMeshProUGUI attackRangeText;
    [SerializeField] private Image replaceImage;
    [SerializeField] private Sprite attackIcon;
    [SerializeField] private Sprite healIcon;

    public void Enable(CommonActions unitSelected)
    {
        gameObject.SetActive(true);
        unitNameText.SetText(unitSelected.SO_BaseUnit.Name);
        moveSpeedText.SetText(unitSelected.GetComponent<NavMeshAgent>().speed.ToString());
        // Only for attackers
        if(unitSelected is BaseAttacker)
        {
            BaseAttacker attacker = unitSelected as BaseAttacker;
            attackDamageText.SetText(attacker.AttackInfo.AttackDamage.ToString());
            attackRangeText.SetText(attacker.AttackInfo.AttackRange.ToString());
            attackSpeedText.SetText(attacker.AttackInfo.AttackSpeed.ToString());
            replaceImage.sprite = attackIcon;
        }
        // Only for healers
        else if(unitSelected is BaseHealer)
        {
            BaseHealer healer = unitSelected as BaseHealer;
            attackDamageText.SetText(healer.HealInfo.Amount.ToString());
            attackRangeText.SetText(healer.HealInfo.HealRange.ToString());
            attackSpeedText.SetText(healer.HealInfo.HealSpeed.ToString());
            replaceImage.sprite = healIcon;
        }
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
