using TMPro;
using UnityEngine;

public class UIResources : MonoBehaviour
{
    // UI resource texts
    [SerializeField] private TextMeshProUGUI foodText;
    [SerializeField] private TextMeshProUGUI woodText;
    [SerializeField] private TextMeshProUGUI stoneText;
    [SerializeField] private TextMeshProUGUI ironText;
    [SerializeField] private TextMeshProUGUI electricityText;
    [SerializeField] private TextMeshProUGUI populationText;
    // Resources types
    [SerializeField] private SO_Resource food;
    [SerializeField] private SO_Resource wood;
    // Resources values

    // Events
    ResourceEvent resourceEvent;

    private void Awake()
    {
        resourceEvent.Register(UpdateResource);
    }

    private void OnDestroy()
    {
        resourceEvent.Unregister(UpdateResource);
    }

    private void UpdateResource(SO_Resource resource)
    {

    }
}
