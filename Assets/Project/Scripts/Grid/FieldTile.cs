using System;
using System.Collections.Generic;
using Project.Scripts.Gameplay.Configs;
using Project.Scripts.Grid.TileUI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.Grid
{
    public class FieldTile : MonoBehaviour
    {
        [SerializeField] private GameObject _buildingMesh;
        [SerializeField] private GameObject _biomMesh;
        [SerializeField] private GameObject _fog;
        [SerializeField] private GameObject _outline;

        [SerializeField] private TileUIView _tileUIView;

        [SerializeField] private Animator animator;

        
        public bool IsUnderFog;
        public bool HasBuilding;
        public bool Selected;

        public bool HaveResources;
        public event Action<FieldTile> OnClicked;

        private Vector2Int _position;

        private TileType _tileType=TileType.Forest;


        public float _fireStickDeltaTime;

        public float _productionDeltaTime;
        public float _consumesDeltaTime;
        private BuildingConfig _buildingConfig;

        public BuildingConfig BuildingConfig => _buildingConfig;

        public void Setup(Vector2Int position, TileType tileType)
        {
            _position = position;
            _tileType = tileType;

            _buildingConfig = GameplayManager.Instance.gameConfig.GetBuildingConfig(_tileType);

            UpdateView();
            ResetTimes();
        }

        public bool TryBuyTorch()
        {
            var canBuy = TryBuy(GameplayManager.Instance.gameConfig.MatchstickPlaceCost);
            if (canBuy)
            {
                //Add your logic
            }
            return canBuy;
        }

        public bool TryBuild()
        {
            var canBuy = TryBuy(_buildingConfig.BuildingCost);
            if (canBuy)
            {
                //Add your logic
            }
            return canBuy;
        }

        public bool TryRefillTorch()
        {
            float torchFillPercent = 1;
            var currentResourceAmounts = GameplayManager.Instance.GetCurrentTorchRefuelCost(torchFillPercent);

            var canBuy = TryBuy(currentResourceAmounts);
            if (canBuy)
            {
                //Add your logic
            }
            return canBuy;
        }


        private bool TryBuy(ResourceAmount[] resourceAmounts)
        {
            bool canBuyTorch = true;
            foreach (var cost in resourceAmounts)
            {
                if (!GameplayManager.Instance.HaveEnoughResource(cost.ResourceType, cost.Amount))
                {
                    canBuyTorch = false;
                    return false;
                }
            }

            foreach (var cost in resourceAmounts)
            {
                GameplayManager.Instance.RemoveResource(cost.ResourceType, cost.Amount);
            }

            return true;
        }

        private void Update()
        {
            TickTimes(Time.deltaTime);
        }

        public float GetTorchProgress()
        {
            float torchDuration = GameplayManager.Instance.gameConfig.MatchStickDuration;
            return (torchDuration - _fireStickDeltaTime) / torchDuration;
        }

        private void TickTimes(float deltaTime)
        {
            if (!IsUnderFog)
            {
                _fireStickDeltaTime += deltaTime;
                float torchDuration = GameplayManager.Instance.gameConfig.MatchStickDuration;
                _tileUIView.UpdateTorchBar((torchDuration - _fireStickDeltaTime) / torchDuration);

                if (_fireStickDeltaTime >= torchDuration)
                {
                    _fireStickDeltaTime = torchDuration;
                    IsUnderFog = true;
                    UpdateView();
                }
            }

            if (HasBuilding||_tileType is TileType.Capital)
            {
                bool haveResources = true;

                if (_buildingConfig.Consumes != null)
                {
                    _consumesDeltaTime += deltaTime;
                    if (_consumesDeltaTime > _buildingConfig.Consumes.IntervalSeconds&&_buildingConfig.Consumes.IntervalSeconds>0)
                    {
                        _consumesDeltaTime = 0;

                        if (GameplayManager.Instance.HaveEnoughResource(_buildingConfig.Consumes.ResourceType,
                                _buildingConfig.Consumes.Amount))
                        {
                            GameplayManager.Instance.RemoveResource(_buildingConfig.Consumes.ResourceType,
                                _buildingConfig.Consumes.Amount);
                        }
                        else
                        {
                            haveResources = false;
                        }
                    }
                }

                if (_buildingConfig.Produces != null)
                {
                    _productionDeltaTime += deltaTime;
                    if (_productionDeltaTime > _buildingConfig.Produces.IntervalSeconds&&_buildingConfig.Produces.IntervalSeconds>0)
                    {
                        _productionDeltaTime = 0;
                        if (haveResources)
                        {
                            GameplayManager.Instance.AddResource(_buildingConfig.Produces.ResourceType,
                                _buildingConfig.Produces.Amount);
                        }
                    }
                }
            }
        }


        private void ResetTimes()
        {
            _fireStickDeltaTime = 0f;
            _productionDeltaTime = 0f;
            _consumesDeltaTime = 0f;
        }

        public void ResetTorch()
        {
            _fireStickDeltaTime = 0f;
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
            if (Selected)
            {
                ClearSelection();
                return;
            }
            Debug.Log($"Mouse Down {_position} {_tileType}");
            
            _tileUIView.ActionsHolder.gameObject.SetActive(true);
            _tileUIView.Setup(IsUnderFog, HasBuilding);
            OnClicked?.Invoke(this);
            Selected = true;
        }

        public void ClearSelection()
        {
            _tileUIView.ActionsHolder.gameObject.SetActive(false);
            
            Selected = false;
        }

        public void RefillTorch()
        {
            ResetTorch();

        }
        public void PlaceTorch()
        {
            IsUnderFog = false;
            animator.SetTrigger("Disperse");
            Debug.Log("Disperse");
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
                _tileUIView.TorchHolder.gameObject.SetActive(false);

            }
            else
            {
                //_fog.SetActive(false);
                _tileUIView.TorchHolder.gameObject.SetActive(true);

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