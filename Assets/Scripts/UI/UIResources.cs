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
    [SerializeField] private SO_Resource so_population;
    // Events
    [SerializeField] private ResourceEvent resourceEvent;
    // Resources values
    public static int Food { get; private set; } = 0;
    public static int MaxFood { get; private set; } = 15;
    public static int Wood { get; private set;} = 0;
    public static int MaxWood { get; private set; } = 1000;
    public static int Stone { get; private set; } = 0;
    public static int MaxStone { get; private set; } = 500;
    public static int Iron { get; private set; } = 0;
    public static int MaxIron { get; private set; } = 500;
    public static int Electricity { get; private set; } = 0;
    public static int MaxElectricity { get; private set; } = 20;
    public static int Population { get; private set; } = 0;
    public static int MaxPopulation { get; private set; } = 10;


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
            MaxFood = resource.IncreaseMaxAmount;
            foodText.SetText(Food.ToString());
        }
        else if(resource == so_wood)
        {
            Wood += resource.ObtainedAmount;
            MaxWood = resource.IncreaseMaxAmount;
            woodText.SetText(Wood.ToString());
        }
        else if(resource == so_iron)
        {
            Iron += resource.ObtainedAmount;
            MaxIron = resource.IncreaseMaxAmount;
            ironText.SetText(Iron.ToString());
        }
        else if(resource == so_electricity)
        {
            Electricity += resource.ObtainedAmount;
            MaxElectricity = resource.IncreaseMaxAmount;
            electricityText.SetText(Electricity.ToString());
        }
        else if(resource == so_population)
        {
            Population += resource.ObtainedAmount;
            MaxPopulation = resource.IncreaseMaxAmount;
            populationText.SetText(Population.ToString());
        }
    }
}
