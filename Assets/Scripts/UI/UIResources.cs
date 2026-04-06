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
        UpdateUI();
    }

    private void OnDestroy()
    {
        resourceEvent.Unregister(UpdateResource);
    }

    private void UpdateResource(ResourceOP resource)
    {
        if(resource.SO_Resource == so_food)
        {
            Food += resource.Amount;
            MaxFood += resource.MaxAmount;
            
        }
        else if(resource.SO_Resource == so_wood)
        {
            Wood += resource.Amount;
            MaxWood = resource.MaxAmount;            
        }
        else if(resource.SO_Resource == so_iron)
        {
            Iron += resource.Amount;
            MaxIron += resource.MaxAmount;            
        }
        else if(resource.SO_Resource == so_electricity)
        {
            Electricity += resource.Amount;
            MaxElectricity += resource.MaxAmount;            
        }
        else if(resource.SO_Resource == so_population)
        {
            Population += resource.Amount;
            MaxPopulation += resource.MaxAmount;            
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        foodText.SetText(Food.ToString() + " / " + MaxFood.ToString());
        woodText.SetText(Wood.ToString() + " / " + MaxIron.ToString());
        ironText.SetText(Iron.ToString() + " / " + MaxIron.ToString());
        electricityText.SetText(Electricity.ToString() + " / " + MaxElectricity.ToString());
        populationText.SetText(Population.ToString() + " / " + MaxPopulation.ToString());
    }
}
