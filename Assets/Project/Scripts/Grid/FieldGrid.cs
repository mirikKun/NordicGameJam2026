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
        [SerializeField] private Vector2Int _fogSize = new Vector2Int(23, 23);
        [SerializeField] private Vector2 _fogHeightRange = new Vector2(-0.2f, 0.4f);

        [SerializeField] private GameObject _fogTiles;
        private FieldTile[,] _grid;

        private void Start()
        {
            CreateGrid();
            
        }

        private void CreateGrid()
        {
            _grid = new FieldTile[_gridSize.x, _gridSize.y];

            for (int x = 0; x < _fogSize.x; x++)
            {
                for (int y = 0; y < _fogSize.y; y++)
                {
                    bool isPlayable = IsInPlayableGrid(x, y);

                    Vector3 position = GetWorldPosition(x, y, _fogSize);

                    if (isPlayable)
                    {
                        int px = x - (_fogSize.x - _gridSize.x) / 2;
                        int py = y - (_fogSize.y - _gridSize.y) / 2;

                        TileType type = GetTileType(px, py);
                        FieldTile prefab = GetPrefabByType(type);

                        if (prefab != null)
                        {
                            FieldTile tile = Instantiate(prefab, position, Quaternion.identity, transform);
                            _grid[px, py] = tile;
                            tile.Setup(new Vector2Int(px, py), type);
                            tile.OnClicked += DeselectOthers;
                        }
                    }
                    else
                    {
                        Instantiate(_fogTiles, position + Vector3.up * Random.Range(_fogHeightRange.x, _fogHeightRange.y), Quaternion.identity, transform);                    }
                }
            }
        }

        private void DeselectOthers(FieldTile selectedTile)
        {
            foreach (var tile in _grid)
            {
                if (selectedTile == tile)
                {
                    if (selectedTile.Selected)
                    {
                        tile.ClearSelection();
                    }
                }
                else
                {
                    tile.ClearSelection();

                }
            }
        }

        private bool IsInPlayableGrid(int fogX, int fogY)
        {
            int offsetX = (_fogSize.x - _gridSize.x) / 2;
            int offsetY = (_fogSize.y - _gridSize.y) / 2;

            return fogX >= offsetX && fogX < offsetX + _gridSize.x &&
                   fogY >= offsetY && fogY < offsetY + _gridSize.y;
        }

        private Vector3 GetWorldPosition(int x, int y, Vector2Int gridSize)
        {
            return transform.position + new Vector3(
                x * (_cellSize.x + _cellOffset.x),
                0f,
                y * (_cellSize.y + _cellOffset.y)
            ) - new Vector3(
                gridSize.x / 2f * (_cellSize.x + _cellOffset.x),
                0f,
                gridSize.y / 2f * (_cellSize.y + _cellOffset.y)
            );
        }

        private TileType GetTileType(int x, int y)
        {
            if (x == _capitalPosition.x && y == _capitalPosition.y)
                return TileType.Capital;

            return TileType.Forest;
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