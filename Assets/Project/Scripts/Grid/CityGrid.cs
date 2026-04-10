using System;
using UnityEngine;

namespace Project.Scripts.Grid
{
    public class CityGrid:MonoBehaviour
    {
    
        [SerializeField] private Vector2Int _gridSize=new Vector2Int(10,10);
        [SerializeField] private Vector2 _cellSize=new Vector2(100f,100f);
        [SerializeField] private Vector2 _cellOffset=new Vector2(100f,100f);

        private void Start()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
        }
    }
}
