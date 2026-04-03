using Mono.Cecil;
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
    [SerializeField] private SO_Resource so_food;
    [SerializeField] private SO_Resource so_wood;
    [SerializeField] private SO_Resource so_stone;
    [SerializeField] private SO_Resource so_iron;
    [SerializeField] private SO_Resource so_electricity;
    // Events
    [SerializeField] private ResourceEvent resourceEvent;
    // Resources values
    public static int Food { get; private set; } = 0;
    public static int Wood { get; private set;} = 0;
    public static int Stone { get; private set; } = 0;
    public static int Iron { get; private set; } = 0;
    public static int Electricity { get; private set; } = 0;



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
        if(resource == so_food)
        {
            Food += resource.ObtainedAmount;
            foodText.SetText(Food.ToString());
        }
        else if(resource == so_wood)
        {
            Wood += resource.ObtainedAmount;
            woodText.SetText(Wood.ToString());
        }
        else if(resource == so_iron)
        {
            Iron += resource.ObtainedAmount;
            ironText.SetText(Iron.ToString());
        }
        else if(resource == so_electricity)
        {
            Electricity += resource.ObtainedAmount;
            electricityText.SetText(Electricity.ToString());
        }
    }
}
