using System;
using Project.Scripts.Grid.TileUI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.Grid
{
    public class FieldTile : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _tileMesh;
        [SerializeField] private GameObject _buildingMesh;
        [SerializeField] private GameObject _biomMesh;
        [SerializeField] private GameObject _fog;
        [SerializeField] private GameObject _outline;

        [SerializeField] private TileUIView _tileUIView;
        
        public bool IsUnderFog;
        public bool HasBuilding;
        public event Action<FieldTile> OnClicked;

        private Vector2Int _position;

        private TileType _tileType;

        public bool Selected;
        // [SerializeField]  private Material _tileMaterial;
        // [SerializeField] private Material _buildingMaterial;


        public void Setup(Vector2Int position, TileType tileType)
        {
            _position = position;
            _tileType = tileType;
            UpdateView();
        }

        private void OnMouseOver()
        {
            //Debug.Log($"OnMouseOver {_position} {_tileType}");
        }

        private void OnMouseExit()
        {
            _outline.SetActive(false);
            // Debug.Log($"E OnMouseOver {_position} {_tileType}");
        }

        private void OnMouseEnter()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            _outline.SetActive(true);
            // Debug.Log($" En  OnMouseOver {_position} {_tileType}");
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            Debug.Log($"Mouse Down {_position} {_tileType}");
            _tileUIView.gameObject.SetActive(true);
            _tileUIView.Setup(IsUnderFog, HasBuilding);
            OnClicked?.Invoke(this);
            Selected = true;
        }

        public void ClearSelection()
        {
            _tileUIView.gameObject.SetActive(false);
            Selected = false;
        }

        public void PlaceTorch()
        {
            IsUnderFog = false;
            UpdateView();
        }

        public void Build()
        {
            HasBuilding = true;
            UpdateView();
        }

        public void UpdateView()
        {
            if (IsUnderFog)
            {
                _fog.SetActive(true);
                _buildingMesh.SetActive(false);
                _biomMesh.SetActive(false);
            }
            else
            {
                _fog.SetActive(false);
                if (HasBuilding)
                {
                    _buildingMesh.SetActive(true);
                    _biomMesh.SetActive(true);
                }
                else
                {
                    _buildingMesh.SetActive(false);
                    _biomMesh.SetActive(true);
                }
            }
        }
    }
}