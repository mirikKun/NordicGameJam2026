using Project.Scripts.Gameplay.Configs;
using Project.Scripts.Grid.Config;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager: MonoBehaviour
{
    public static GameplayManager Instance;
    public GameConfig gameConfig;
    public StartBoardConfig startBoardConfig;
    public TextMeshProUGUI woodAmount;
    public TextMeshProUGUI stoneAmount;
    public TextMeshProUGUI foodAmount;
    

    public Button AddWoodButton;
    public Button AddStoneButton;
    public Button AddFoodButton;

    public Button RemoveWoodButton;
    public Button RemoveStoneButton;
    public Button RemoveFoodButton;

    public int addResourceAmount = 5;


    private void Start()
    {
        Instance = this;
        
        Resources.woodCount = gameConfig.StartWood.Amount;
        Resources.foodCount = gameConfig.StartFood.Amount;
        Resources.stoneCount = gameConfig.StartStone.Amount;

        woodAmount.SetText(Resources.woodCount.ToString());
        foodAmount.SetText(Resources.foodCount.ToString());
        stoneAmount.SetText(Resources.stoneCount.ToString());

        //For testing
        AddWoodButton.onClick.AddListener(() => AddResource(ResourceType.Wood, addResourceAmount));
        AddStoneButton.onClick.AddListener(() => AddResource(ResourceType.Stone, addResourceAmount));
        AddFoodButton.onClick.AddListener(() => AddResource(ResourceType.Food, addResourceAmount));

        RemoveWoodButton.onClick.AddListener(() => RemoveResource(ResourceType.Wood, addResourceAmount));
        RemoveStoneButton.onClick.AddListener(() => RemoveResource(ResourceType.Stone, addResourceAmount));
        RemoveFoodButton.onClick.AddListener(() => RemoveResource(ResourceType.Food, addResourceAmount));
    }
    public ResourceAmount[] GetCurrentTorchRefuelCost(float torchFillPercent)
    {
        ResourceAmount[] maxResourceAmounts = GameplayManager.Instance.gameConfig.MatchstickRefuelFullCost;
        ResourceAmount[] currentResourceAmounts=new ResourceAmount[maxResourceAmounts.Length];
        for (var i = 0; i < currentResourceAmounts.Length; i++)
        {
            var resourceAmount = currentResourceAmounts[i];
            resourceAmount.ResourceType = maxResourceAmounts[i].ResourceType;
            resourceAmount.Amount = (int)torchFillPercent*resourceAmount.Amount;
        }

        return currentResourceAmounts;
    }
    public void AddResource(ResourceType resource, int amount)
    {
        switch(resource)
        {
            case ResourceType.Wood:
                Resources.woodCount += amount;
                break;
            case ResourceType.Stone:
                Resources.stoneCount += amount;
                break;
            case ResourceType.Food:
                Resources.foodCount += amount;
                break;
        }

        Debug.Log("Resources added");

        DisplayResources(resource);
    }

    public void RemoveResource(ResourceType resource, int amount)
    {
        
        switch (resource)
        {
            case ResourceType.Wood:                
                Resources.woodCount -= amount;
                if (Resources.woodCount < 0)
                    Resources.woodCount = 0;
                break;
            case ResourceType.Stone:
                Resources.stoneCount -= amount;
                if(Resources.stoneCount < 0)
                    Resources.stoneCount = 0;
                break;
            case ResourceType.Food:
                Resources.foodCount -= amount;
                    if(Resources.foodCount < 0)
                    Resources.foodCount = 0; 
                break;
        }

        Debug.Log("Resources added");

        DisplayResources(resource);
    }
    public bool HaveEnoughResource(ResourceType resource, int amount)
    {
        return resource switch
        {
            ResourceType.Wood => Resources.woodCount >= amount,
            ResourceType.Stone => Resources.stoneCount >= amount,
            ResourceType.Food => Resources.foodCount >= amount,
            _ => false
        };
    }
    private void DisplayResources(ResourceType resource)
    {
        switch(resource)
        {
            case ResourceType.Wood:
                woodAmount.SetText(Resources.woodCount.ToString());
                break;
            case ResourceType.Stone:
                stoneAmount.SetText(Resources.stoneCount.ToString());
                break;
            case ResourceType.Food:
                foodAmount.SetText(Resources.foodCount.ToString());
                break;
        }
    }


}
public enum ResourceType
{
    Wood,
    Stone,
    Food
}
