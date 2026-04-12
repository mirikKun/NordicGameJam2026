using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Grid.TileUI
{
    public class PriceView : MonoBehaviour
    {
        [SerializeField] private Image _priceImage;
        [SerializeField] private TextMeshProUGUI _priceText;
        public void Setup(Sprite icon, int amount,Color color)
        {
            _priceImage.sprite = icon;
            _priceText.text = amount.ToString();
            //_priceText.color = color;
            _priceImage.color = color;
        }
    }
}