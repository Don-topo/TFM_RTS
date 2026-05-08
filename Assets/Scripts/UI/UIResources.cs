using Mono.Cecil;
using TMPro;
using UnityEngine;

public class UIResources : MonoBehaviour
{
    // UI resource texts
    [Header("Icons")]
    [SerializeField] private TextMeshProUGUI foodText;
    [SerializeField] private TextMeshProUGUI woodText;
    [SerializeField] private TextMeshProUGUI stoneText;
    [SerializeField] private TextMeshProUGUI ironText;
    [SerializeField] private TextMeshProUGUI electricityText;
    [SerializeField] private TextMeshProUGUI populationText;
    // Events
    [Header("Events")]
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
        switch (resource.SO_Resource.ResourceTypes)
        {
            case ResourcesType.Food:
                Food = Mathf.Clamp(Food + resource.Amount, 0, MaxFood);
                MaxFood += resource.MaxAmount;
                break;
            case ResourcesType.Wood:
                Wood = Mathf.Clamp(Wood + resource.Amount, 0, MaxWood);
                MaxWood += resource.MaxAmount;
                break;
            case ResourcesType.Stone:
                Stone = Mathf.Clamp(Stone + resource.Amount, 0, MaxStone);
                MaxStone += resource.MaxAmount;
                break;
            case ResourcesType.Iron:
                Iron = Mathf.Clamp(Iron + resource.Amount, 0, MaxIron);
                MaxIron += resource.MaxAmount;
                break;
            case ResourcesType.Electricity:
                Electricity = Mathf.Clamp(Electricity + resource.Amount, 0, MaxElectricity);
                MaxElectricity += resource.MaxAmount;
                break;
            case ResourcesType.Population:
                Population = Mathf.Clamp(Population + resource.Amount, 0, MaxPopulation);
                MaxPopulation += resource.MaxAmount;
                break;
            default:
                break;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        foodText.color = Food >= MaxFood ? Color.red : Color.white;
        foodText.SetText(Food.ToString() + " / " + MaxFood.ToString());
        woodText.color = Wood >= MaxWood ? Color.red : Color.white;
        woodText.SetText(Wood.ToString() + " / " + MaxWood.ToString());
        ironText.color = Iron >= MaxIron ? Color.red : Color.white;
        ironText.SetText(Iron.ToString() + " / " + MaxIron.ToString());
        stoneText.color = Stone >= MaxStone ? Color.red : Color.white;
        stoneText.SetText(Stone.ToString() + " / " + MaxStone.ToString());
        electricityText.color = Electricity >= MaxElectricity ? Color.red : Color.white;
        electricityText.SetText(Electricity.ToString() + " / " + MaxElectricity.ToString());
        populationText.color = Population >= MaxPopulation ? Color.red : Color.white;
        populationText.SetText(Population.ToString() + " / " + MaxPopulation.ToString());
    }
}
