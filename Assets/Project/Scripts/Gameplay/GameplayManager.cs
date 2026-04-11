using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager: MonoBehaviour
{
    public TextMeshProUGUI woodAmount;
    public TextMeshProUGUI stoneAmount;
    public TextMeshProUGUI foodAmount;

    public int woodStartingAmount = 0;
    public int stoneStartingAmount = 0;
    public int foodStartingAmount = 0;

    public Button AddWoodButton;
    public Button AddStoneButton;
    public Button AddFoodButton;

    public Button RemoveWoodButton;
    public Button RemoveStoneButton;
    public Button RemoveFoodButton;

    public int addResourceAmount = 5;

    public enum ResourceType
    {
        Wood,
        Stone,
        Food
    }
    private void Start()
    {
        Resources.woodCount = woodStartingAmount;
        Resources.foodCount = foodStartingAmount;
        Resources.stoneCount = stoneStartingAmount;

        woodAmount.SetText(woodStartingAmount.ToString());
        stoneAmount.SetText(stoneStartingAmount.ToString());
        foodAmount.SetText(foodStartingAmount.ToString());

        //For testing
        AddWoodButton.onClick.AddListener(() => AddResource(ResourceType.Wood, addResourceAmount));
        AddStoneButton.onClick.AddListener(() => AddResource(ResourceType.Stone, addResourceAmount));
        AddFoodButton.onClick.AddListener(() => AddResource(ResourceType.Food, addResourceAmount));

        RemoveWoodButton.onClick.AddListener(() => RemoveResource(ResourceType.Wood, addResourceAmount));
        RemoveStoneButton.onClick.AddListener(() => RemoveResource(ResourceType.Stone, addResourceAmount));
        RemoveFoodButton.onClick.AddListener(() => RemoveResource(ResourceType.Food, addResourceAmount));


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
