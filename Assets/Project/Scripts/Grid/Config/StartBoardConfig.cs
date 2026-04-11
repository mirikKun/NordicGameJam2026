using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Grid.Config
{
    [CreateAssetMenu(fileName = "StartBoardConfig", menuName = "Config/StartBoardConfig")]
    public class StartBoardConfig:ScriptableObject
    {
        [SerializeField]public List<BoardRow> TileTypes=new List<BoardRow>()
        {
           new( new List<TileType>(){TileType.Mountains,TileType.Mountains,TileType.Mountains,TileType.Forest,TileType.Mountains}),
           new( new List<TileType>(){TileType.Mountains,TileType.Field,TileType.Field,TileType.Forest,TileType.Mountains}),
               new( new List<TileType>(){TileType.Forest,TileType.Field,TileType.Capital,TileType.Field,TileType.Field }),
                   new( new List<TileType>(){TileType.Forest,TileType.Forest,TileType.Forest,TileType.Forest,TileType.Mountains}),
                       new( new List<TileType>(){TileType.Mountains,TileType.Field,TileType.Field,TileType.Mountains,TileType.Mountains})
        };

        [Serializable]
        public class BoardRow
        {
            public List<TileType> Row;

            public BoardRow(List<TileType> row)
            {
                Row = row;
            }
        }
    }
}