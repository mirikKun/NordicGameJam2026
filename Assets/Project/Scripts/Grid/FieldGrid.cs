using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project.Scripts.Grid
{
    public class FieldGrid : MonoBehaviour
    {
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(10, 10);
        [SerializeField] private Vector2 _cellSize = new Vector2(100f, 100f);
        [SerializeField] private Vector2 _cellOffset = new Vector2(100f, 100f);

        [SerializeField] private Vector2Int _capitalPosition = new Vector2Int(5, 5);

        [SerializeField] private CellTypes[] _cellTypes;
        private FieldTile[,] _grid;

        private void Start()
        {
            CreateGrid();
        }

        private void CreateGrid()
        {
            _grid = new FieldTile[_gridSize.x, _gridSize.y];

            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    TileType type = GetTileType(x, y);
                    FieldTile prefab = GetPrefabByType(type);

                    if (prefab == null) continue;

                    Vector3 position = transform.position+new Vector3(
                        x * (_cellSize.x + _cellOffset.x),
                        0f,
                        y * (_cellSize.y + _cellOffset.y)
                    )-new Vector3(
                        _gridSize.x/2f * (_cellSize.x + _cellOffset.x),
                        0f,
                        _gridSize.y/2f * (_cellSize.y + _cellOffset.y)
                    );

                    FieldTile tile = Instantiate(prefab, position, Quaternion.identity, transform);
                    _grid[x, y] = tile;
                }
            }
        }

        private TileType GetTileType(int x, int y)
        {
            if (x == _capitalPosition.x && y == _capitalPosition.y)
                return TileType.Capital;

            return TileType.Field;
            TileType[] nonCapital = { TileType.Forest, TileType.Field, TileType.Mountains };
            return nonCapital[Random.Range(0, nonCapital.Length)];
        }

        private FieldTile GetPrefabByType(TileType type)
        {
            foreach (CellTypes cell in _cellTypes)
            {
                if (cell.TileType == type)
                    return cell.FieldTile;
            }

            return null;
        }
    }

    [Serializable]
    public class CellTypes
    {
        public TileType TileType;
        public FieldTile FieldTile;
    }

    public enum TileType
    {
        Capital,
        Forest,
        Field,
        Mountains
    }
}