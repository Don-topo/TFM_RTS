using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Composites;
using UnityEngine.UI;

public class UIRecruitButton : MonoBehaviour
{
    [SerializeField] private Image image;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        Disable();
    }

    public void Enable(SO_BaseUnit unit, UnityAction unityAction)
    {
        // Safety remove listeners, just in case
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(unityAction);
        button.interactable = true;
        // Set queue button icon
        image.sprite = unit.Icon;
        image.gameObject.SetActive(true);
    }

    public void Disable()
    {
        button.onClick.RemoveAllListeners();
        button.interactable = false;
        image.sprite = null;
    }
}
