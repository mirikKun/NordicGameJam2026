using System;
using System.Collections.Generic;
using Project.Scripts.Grid;
using UnityEngine;

namespace Project.Scripts.Gameplay.Configs
{
    [CreateAssetMenu(fileName = "ColorsConfig", menuName = "Configs/ColorsConfig")]
    public class ColorsConfig:ScriptableObject
    {
        public List<TileColor>  TileColors;

        public Color GetTileColor(ResourceType tileType)
        {
            foreach (TileColor tileColor in TileColors)
            {
                if (tileColor.TileType == tileType)
                    return tileColor.Color;
            }
            return Color.white; 
        }
    }
[Serializable]
    public class TileColor
    {
        public Color Color;
        public ResourceType TileType;
    }
}