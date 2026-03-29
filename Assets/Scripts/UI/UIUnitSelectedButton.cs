using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIUnitSelectedButton : MonoBehaviour
{
    [SerializeField] private Image unitImage;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        Disable();
    }

    public void Enable(CommonActions unit, UnityAction unityAction)
    {
        gameObject.SetActive(true);
        unitImage.sprite = unit.so_baseUnit.Icon;
        button.onClick.AddListener(unityAction);
    }

    private void Disable()
    {
        button.onClick.RemoveAllListeners();
        gameObject.SetActive(false);
    }
}
