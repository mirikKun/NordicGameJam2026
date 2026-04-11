using System;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Grid.TileUI
{
    public class TileUIView : MonoBehaviour
    {
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
        

        [SerializeField] private ResIcon[] _resIcons;

    

        private Sprite GetIcon(Res res)
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
                
                CreatePriceView(GetPrice(), _placeTorchPrice);
            }
            else
            {
                _placeTorchRoot.gameObject.SetActive(false);
                _refillTorchRoot.gameObject.SetActive(true);
                CreatePriceView(GetPrice(), _refillTorchPrice);

                _buildRoot.gameObject.SetActive(!hasBuilding);
                if (!hasBuilding)
                {
                    CreatePriceView(GetPrice(), _buildPrice);

                }
            }
        }

        private void TryPlaceTorch()
        {
            if (true) //Check condition with resources
            {
                _fieldTile.PlaceTorch();
            }
        }

        private void TryBuild()
        {
            if (true) //Check condition with resources
            {
                _fieldTile.Build();

            }
        }

        private void TryRefillTorch()
        {
            if (true) //Check condition with resources
            {
            }
        }

        private void CreatePriceView(Price[] prices, RectTransform parent)
        {
            foreach (Transform child in parent)
                Destroy(child.gameObject);

            foreach (Price price in prices)
            {
                PriceView view = Instantiate(_prefab, parent);
                Sprite icon = GetIcon(price.ResType);
                view.Setup(icon, price.Amount);
            }
        }

        public Price[] GetPrice()
        {
            //TODO
            return new Price[]
            {
                new Price(Res.wood, 10),
                new Price(Res.stone, 10),
            };
        }
    }

    public class Price
    {
        public Res ResType;
        public int Amount;

        public Price(Res resType, int amount)
        {
            ResType = resType;
            Amount = amount;
        }
    }

    public enum Res
    {
        stone,
        wood
    }
    [Serializable]
    public class ResIcon
    {
        public Res Res;
        public Sprite Icon;
    }
}