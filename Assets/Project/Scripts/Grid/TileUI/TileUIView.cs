using System;
using Project.Scripts.Animation;
using Project.Scripts.Gameplay.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Grid.TileUI
{
    public class TileUIView : MonoBehaviour
    {   
        public RectTransform ActionsHolder;
        public RectTransform TorchHolder;
        public ResourceGetAnimation AnimationsHolder;
        
        [SerializeField] private FieldTile _fieldTile;
        [SerializeField] private PriceView _prefab;

        [Header("Place torch")] [SerializeField]
        private RectTransform _placeTorchRoot;

        [SerializeField] private RectTransform _placeTorchPrice;
        [SerializeField] private Button _placeTorchButton;

        [Header("Build building")] 
        [SerializeField] private RectTransform _buildRoot;
        [SerializeField] private RectTransform _buildPrice;
        [SerializeField] private Button _buildButton;

        [Header("Refill Torch Price")] 
        [SerializeField] private RectTransform _refillTorchRoot;
        [SerializeField] private RectTransform _refillTorchPrice;
        [SerializeField] private Button _refillTorchButton;
        [Header("Torch")] 
        [SerializeField] private Image _torch; 


        [SerializeField] private ResIcon[] _resIcons;

     
    

        private Sprite GetIcon(ResourceType res)
        {
            foreach (ResIcon resIcon in _resIcons)
            {
                if (resIcon.Res == res)
                    return resIcon.Icon;
            }
            return null;
        }

        
        public void Setup(bool isInFog, bool hasBuilding)
        {
            _placeTorchButton.onClick.RemoveAllListeners();
            _buildButton.onClick.RemoveAllListeners();
            _refillTorchButton.onClick.RemoveAllListeners();


            _buildButton.onClick.AddListener(TryBuild);
            _placeTorchButton.onClick.AddListener(TryPlaceTorch);
            _refillTorchButton.onClick.AddListener(TryRefillTorch);


            if (isInFog)
            {
                _placeTorchRoot.gameObject.SetActive(true);
                _buildRoot.gameObject.SetActive(false);
                _refillTorchRoot.gameObject.SetActive(false);
                
                CreatePriceView(GameplayManager.Instance.gameConfig.MatchstickPlaceCost, _placeTorchPrice);
            }
            else
            {
                _placeTorchRoot.gameObject.SetActive(false);
                _refillTorchRoot.gameObject.SetActive(true);
                CreatePriceView( GameplayManager.Instance.GetCurrentTorchRefuelCost(_fieldTile.GetTorchProgress()), _refillTorchPrice);

                _buildRoot.gameObject.SetActive(!hasBuilding);
                if (!hasBuilding)
                {
                    CreatePriceView(_fieldTile.BuildingConfig.BuildingCost, _buildPrice);

                }
            }
        }

        public void UpdateTorchBar(float percentage)
        {
            _torch.fillAmount = percentage;
        }

        private void TryPlaceTorch()
        {
            if (_fieldTile.TryBuyTorch())
            {
                Debug.Log($"Place torch");
                _fieldTile.PlaceTorch();
                _fieldTile.ClearSelection();

            }
        }

        private void TryBuild()
        {
            if (_fieldTile.TryBuild()) 
            {
                Debug.Log($"Build");
                _fieldTile.Build();
                _fieldTile.ClearSelection();
            }
        }

        private void TryRefillTorch()
        {
            if (_fieldTile.TryRefillTorch()) //Check condition with resources
            {
                _fieldTile.RefillTorch();
                _fieldTile.ClearSelection();

            }
        }

        private void CreatePriceView(ResourceAmount[] prices, RectTransform parent)
        {
            foreach (Transform child in parent)
                Destroy(child.gameObject);

            foreach (ResourceAmount price in prices)
            {
                PriceView view = Instantiate(_prefab, parent);
                Sprite icon = GetIcon(price.ResourceType);
                view.Setup(icon, price.Amount,GameplayManager.Instance.colorsConfig.GetTileColor(price.ResourceType));
            }
        }

   
    }




    [Serializable]
    public class ResIcon
    {
        public ResourceType Res;
        public Sprite Icon;
    }
    
}