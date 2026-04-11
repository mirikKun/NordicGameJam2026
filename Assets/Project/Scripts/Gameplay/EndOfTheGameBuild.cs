using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Gameplay
{
    public class EndOfTheGameBuild:MonoBehaviour
    {
        [SerializeField] private List<EndOfTheGameResultDescription> _descriptions;
        //[SerializeField] private GameObject _winDecorations;
        [SerializeField] private Button _restartButton;
        [SerializeField] private TextMeshProUGUI _descriptionText;

        private void Start()
        {
            _restartButton.onClick.AddListener(GameplayManager.Instance.RestartGame);
        }

        public void SetupWindow(GameResult gameResult)
        {
            
            foreach (var description in _descriptions)
            {
                //_winDecorations.SetActive(gameResult == GameResult.Win);
                if (description.GameResult == gameResult)
                {
                    _descriptionText.text = description.Description;
                    break;
                }
            }
        }
    }

    [Serializable]
    public class EndOfTheGameResultDescription
    {
        public GameResult GameResult;
        public string Description;
    }

    public enum GameResult
    {
        Win,
        LoseNoFood,
        LoseLighthouseLightWentOut
    }
}