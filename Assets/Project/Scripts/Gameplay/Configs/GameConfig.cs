using System;
using Project.Scripts.Grid;
using UnityEngine;

namespace Project.Scripts.Gameplay.Configs
{[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/GameConfig")]

    public class GameConfig : ScriptableObject
    {
        [Header("Starting Resources")]
        public ResourceAmount StartWood =new ResourceAmount(){ResourceType = ResourceType.Wood,Amount = 50} ;
        public ResourceAmount StartFood=new ResourceAmount(){ResourceType = ResourceType.Food,Amount = 50} ;
        public ResourceAmount StartStone=new ResourceAmount(){ResourceType = ResourceType.Stone,Amount = 0} ;

        [Header("Matchstick")] 
        public float MatchStickDuration;
        public ResourceAmount[] MatchstickPlaceCost = new[] { new ResourceAmount() { ResourceType = ResourceType.Wood, Amount = 10 } };
        public ResourceAmount[] MatchstickRefuelFullCost  = new[] { new ResourceAmount() { ResourceType = ResourceType.Wood, Amount = 5 } };
        
        
        [Header("Buildings")]
        public BuildingConfig WoodBuilding = new BuildingConfig
        {
            BuildingCost = new[] { new ResourceAmount { ResourceType = ResourceType.Wood, Amount = 10 } },
            Produces =  new ResourceFlow { ResourceType = ResourceType.Wood, Amount = 1, IntervalSeconds = 3f } ,
            Consumes =  new ResourceFlow { ResourceType = ResourceType.Food, Amount = 1, IntervalSeconds = 5f } 
        };

        public BuildingConfig StoneBuilding = new BuildingConfig
        {
            BuildingCost = new[] { new ResourceAmount { ResourceType = ResourceType.Wood, Amount = 10 } },
            Produces =  new ResourceFlow { ResourceType = ResourceType.Stone, Amount = 1, IntervalSeconds = 5f } ,
            Consumes = new ResourceFlow { ResourceType = ResourceType.Food, Amount = 1, IntervalSeconds = 5f } 
        };

        public BuildingConfig FoodBuilding = new BuildingConfig
        {
            BuildingCost = new[] { new ResourceAmount { ResourceType = ResourceType.Wood, Amount = 10 } },
            Produces =  new ResourceFlow { ResourceType = ResourceType.Food, Amount = 5, IntervalSeconds = 5f } ,
            Consumes = null
        };

        public BuildingConfig Lighthouse = new BuildingConfig
        {
            BuildingCost = new[] { new ResourceAmount { ResourceType = ResourceType.Stone, Amount = 100 } },
            Produces = null,
            Consumes =  new ResourceFlow { ResourceType = ResourceType.Food, Amount = 1, IntervalSeconds = 5f } 
        };


        public BuildingConfig GetBuildingConfig(TileType tileType)
        {
            switch (tileType)
            {
                case TileType.Capital:
                    return Lighthouse;
                    break;
                case TileType.Forest:
                    return WoodBuilding;
                    break;
                case TileType.Field:
                    return FoodBuilding;

                    break;
                case TileType.Mountains:
                    return StoneBuilding;
                    break;
                default:
                    return null;
            }
        }
    }
    
    [Serializable]
    public class ResourceFlow
    {
        public ResourceType ResourceType;
        public int Amount;
        public float IntervalSeconds;
    }

    [Serializable]
    public class BuildingConfig
    {
        public ResourceAmount[] BuildingCost;
        public ResourceFlow Produces;
        public ResourceFlow Consumes;
    }

    [Serializable]
    public class ResourceAmount
    {
        public ResourceType ResourceType;
        public int Amount;

    }
}