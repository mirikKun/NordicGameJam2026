using System;
using Project.Scripts.Animation;
using Project.Scripts.Gameplay;
using Project.Scripts.Gameplay.Configs;
using Project.Scripts.Grid.TileUI;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.Grid
{
    public class FieldTile : MonoBehaviour
    {
        private static readonly int Disperse = Animator.StringToHash("Disperse");
        private static readonly int Return = Animator.StringToHash("Return");


        [SerializeField] private GameObject _buildingMesh;
        [SerializeField] private GameObject _biomMesh;
        [SerializeField] private GameObject _fog;
        [SerializeField] private GameObject _outline;

        [SerializeField] private TileUIView _tileUIView;

        [SerializeField] private Animator animator;
        [SerializeField] private AudioClip torch1;
        [SerializeField] private AudioClip torch2;
        [SerializeField] private AudioClip relightTorch2;
        [SerializeField] private AudioClip burnedOutFire;
        [SerializeField] private AudioSource sfxAudioSource;


        public bool IsUnderFog;
        public bool HasBuilding;
        public bool Selected;

        public bool HaveResources;
        public event Action<FieldTile> OnClicked;

        private Vector2Int _position;
        private AnimationEventReceiver _receiver;

        private TileType _tileType = TileType.Forest;


        public float _fireStickDeltaTime;

        public float _productionDeltaTime;
        public TileType TileType => _tileType;
        public Vector2Int Position => _position;
        public float _consumesDeltaTime;
        private BuildingConfig _buildingConfig;
        private FieldGrid _grid;

        public BuildingConfig BuildingConfig => _buildingConfig;

        private void Awake()
        {
            _receiver = animator.GetComponent<AnimationEventReceiver>();
        }

        public void Setup(FieldGrid grid, Vector2Int position, TileType tileType)
        {
            _grid = grid;
            _position = position;
            _tileType = tileType;

            _buildingConfig = GameplayManager.Instance.gameConfig.GetBuildingConfig(_tileType);

            if (_tileType is TileType.Capital)
            {
                animator.SetTrigger(Disperse);
            }

            UpdateView();
            ResetTimes();
        }

        public bool TryBuyTorch()
        {
            var canBuy = TryBuy(GameplayManager.Instance.gameConfig.MatchstickPlaceCost);
            return canBuy;
        }

        public bool TryBuild()
        {
            var canBuy = TryBuy(_buildingConfig.BuildingCost);
            return canBuy;
        }

        public bool TryRefillTorch()
        {
            float torchFillPercent = 1;
            var currentResourceAmounts = GameplayManager.Instance.GetCurrentTorchRefuelCost(torchFillPercent);
            return TryBuy(currentResourceAmounts);
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
            if (!GameplayManager.Instance.GameInProcess) return;
            TickTorch(deltaTime);


            if (HasBuilding || _tileType is TileType.Capital)
            {
                bool haveResources = true;

                haveResources = TickConsumes(deltaTime);
                TickProduction(deltaTime, haveResources);
            }
        }

        private void TickProduction(float deltaTime, bool haveResources)
        {
            if (_buildingConfig.Produces != null)
            {
                _productionDeltaTime += deltaTime;
                if (_productionDeltaTime > _buildingConfig.Produces.IntervalSeconds &&
                    _buildingConfig.Produces.IntervalSeconds > 0)
                {
                    _productionDeltaTime = 0;
                    if (haveResources)
                    {
                        GameplayManager.Instance.AddResource(_buildingConfig.Produces.ResourceType,
                            _buildingConfig.Produces.Amount);
                        _tileUIView.AnimationsHolder.PlayAnimation("+" + _buildingConfig.Produces.Amount,
                            _tileUIView.GetIcon(_buildingConfig.Produces.ResourceType),
                            GameplayManager.Instance.colorsConfig.GetTileColor(_buildingConfig.Produces.ResourceType));
                    }
                }
            }
        }

        private bool TickConsumes(float deltaTime)
        {
            bool haveResources = true;
            if (_buildingConfig.Consumes != null)
            {
                _consumesDeltaTime += deltaTime;
                if (_consumesDeltaTime > _buildingConfig.Consumes.IntervalSeconds &&
                    _buildingConfig.Consumes.IntervalSeconds > 0)
                {
                    _consumesDeltaTime = 0;

                    if (GameplayManager.Instance.HaveEnoughResource(_buildingConfig.Consumes.ResourceType,
                            _buildingConfig.Consumes.Amount))
                    {
                        GameplayManager.Instance.RemoveResource(_buildingConfig.Consumes.ResourceType,
                            _buildingConfig.Consumes.Amount);
                        _tileUIView.AnimationsHolder.PlayAnimation("-" + _buildingConfig.Consumes.Amount,
                            _tileUIView.GetIcon(_buildingConfig.Produces.ResourceType),
                            GameplayManager.Instance.colorsConfig.GetTileColor(_buildingConfig.Consumes.ResourceType));
                    }
                    else
                    {
                        if (_buildingConfig.Consumes.ResourceType == ResourceType.Food)
                        {
                            GameplayManager.Instance.FinishGame(GameResult.LoseNoFood);
                        }

                        haveResources = false;
                    }
                }
            }

            return haveResources;
        }

        private void TickTorch(float deltaTime)
        {
            if (!IsUnderFog)
            {
                _fireStickDeltaTime += deltaTime;
                float torchDuration = GameplayManager.Instance.gameConfig.MatchStickDuration;
                _tileUIView.UpdateTorchBar((torchDuration - _fireStickDeltaTime) / torchDuration);
                if (_tileType is TileType.Capital)
                {
                    GameplayManager.Instance.LightHouseIndicator.Tick((torchDuration - _fireStickDeltaTime) /
                                                                      torchDuration);
                }

                if (_fireStickDeltaTime >= torchDuration)
                {
                    _fireStickDeltaTime = torchDuration;
                    ReturnFog();
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
            //UpdateView();
        }

        private void OnMouseExit()
        {
            _outline.SetActive(false);
            // Debug.Log($"E OnMouseOver {_position} {_tileType}");
        }

        private void OnMouseEnter()
        {
            if (CanClick() || !IsCloseToOther()) return;

            _outline.SetActive(true);
            // Debug.Log($" En  OnMouseOver {_position} {_tileType}");
        }

        private void OnMouseDown()
        {
            if (CanClick() || !IsCloseToOther()) return;
            if (Selected)
            {
                ClearSelection();
                return;
            }

            Debug.Log($"Mouse Down {_position} {_tileType}");

            _tileUIView.StartAppearAnimation();
            _tileUIView.Setup(IsUnderFog, HasBuilding);
            OnClicked?.Invoke(this);
            Selected = true;
        }

        private static bool CanClick()
        {
            if (Input.touchCount > 0)
                return EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId);

            return EventSystem.current.IsPointerOverGameObject();
        }

        private bool IsCloseToOther()
        {
            if (!IsUnderFog) return true;
            Vector2Int currentPosition = _position;
            FieldTile[,] grid = _grid.Grid;

            Vector2Int[] neighbors =
            {
                new Vector2Int(currentPosition.x - 1, currentPosition.y),
                new Vector2Int(currentPosition.x + 1, currentPosition.y),
                new Vector2Int(currentPosition.x, currentPosition.y - 1),
                new Vector2Int(currentPosition.x, currentPosition.y + 1)
            };

            foreach (Vector2Int neighbor in neighbors)
            {
                if (neighbor.x < 0 || neighbor.x >= grid.GetLength(0)) continue;
                if (neighbor.y < 0 || neighbor.y >= grid.GetLength(1)) continue;

                if (!grid[neighbor.x, neighbor.y].IsUnderFog)
                    return true;
            }

            return false;
        }

        public void ClearSelection()
        {
            _tileUIView.StopAppearAnimation();

            Selected = false;
        }

        public void RefillTorch()
        {
            //Play sfx audio
            PlayAudioClipFromArray(new[] { torch1, torch2 });
            ResetTorch();
        }

        public void PlaceTorch(bool withSound=true)
        {
            //Play sfx audio
            if (withSound)
            PlayAudioClipFromArray(new[] { torch1, torch2 });

            ResetTimes();
            IsUnderFog = false;
            Debug.Log("Disperse");
            // animator.SetTrigger(Disperse);
            // UpdateView();
            StartAnimation(Disperse);
        }

        private void ReturnFog()
        {
            IsUnderFog = true;
            if (GameplayManager.Instance.gameConfig.CanFogDestroyBuildings)
            {
                HasBuilding = false;
            }

            StartAnimation(Return);
            PlayAudioClip(burnedOutFire);

            if (_tileType == TileType.Capital)
            {
                GameplayManager.Instance.FinishGame(GameResult.LoseLighthouseLightWentOut);
            }
        }

        private void StartAnimation(int animationHash)
        {
            _receiver.Called -= OnEvent;

            animator.SetTrigger(animationHash);
            _receiver.Called += OnEvent;
        }

        private void OnEvent()
        {
            _receiver.Called -= OnEvent;

            UpdateView();
        }

        public void Build()
        {
            HasBuilding = true;
            UpdateView();

            if (_tileType == TileType.Capital)
            {
                GameplayManager.Instance.FinishGame(GameResult.Win);
            }
        }

        public void PlayAudioClip(AudioClip sfx)
        {
            sfxAudioSource.clip = sfx;
            sfxAudioSource.Play();

            //sfxAudioSource.PlayOneShot(sfx);
        }

        public void PlayAudioClipFromArray(AudioClip[] sfxArray)
        {
            int index = UnityEngine.Random.Range(0, sfxArray.Length);

            sfxAudioSource.clip = sfxArray[index];
            sfxAudioSource.Play();
            //sfxAudioSource.PlayOneShot(sfxArray[index]);
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
                _fog.SetActive(false);
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