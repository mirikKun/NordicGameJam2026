using System;
using Project.Scripts.Grid.TileUI;
using UnityEngine;

namespace Project.Scripts.Grid
{
    public class FieldTile : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _tileMesh;
        [SerializeField] private MeshRenderer _buildingMesh;
        [SerializeField] private MeshRenderer _biomMesh;
        [SerializeField] private GameObject _fog;

        [SerializeField] private TileUIView _tileUIView;
        public bool IsUnderFog;
        public bool HasBuilding;

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
        private void OnMouseEnter()
        {
            Debug.Log($" En  OnMouseOver {_position} {_tileType}");
            _tileUIView.gameObject.SetActive(true);
            _tileUIView.Setup(IsUnderFog,HasBuilding);
            
        }
        private void OnMouseExit()
        {
            Debug.Log($"E OnMouseOver {_position} {_tileType}");
            _tileUIView.gameObject.SetActive(false);

        }

        public void PlaceTorch()
        {
            IsUnderFog = false;
            _fog.gameObject.SetActive(false);
            
        }

        public void Build()
        {
            HasBuilding = false;
            _buildingMesh.gameObject.SetActive(true);
        }
    }
}