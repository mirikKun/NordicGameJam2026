using System;
using UnityEngine;

namespace Project.Scripts.Grid
{
    public class FieldTile : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _tileMesh;
        [SerializeField] private MeshRenderer _buildingMesh;
        [SerializeField] private MeshRenderer _biomMesh;

        public bool IsUnderFog;

        private Vector2Int _position;

        private TileType _tileType;
        // [SerializeField]  private Material _tileMaterial;
        // [SerializeField] private Material _buildingMaterial;


        public void Setup(Vector2Int position, TileType tileType)
        {
            _position = position;
            _tileType= tileType;
        }
        private void OnMouseOver()
        {
            Debug.Log($"OnMouseOver {_position} {_tileType}");
        }
    }
}