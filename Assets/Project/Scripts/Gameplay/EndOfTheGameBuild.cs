using System;
using System.Collections;
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
        [SerializeField] private TextMeshProUGUI _titleText;

        
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private AnimationCurve _appearCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private float _animationDuration = 0.5f;
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
                    _titleText.text = description.Title;
                    _descriptionText.text = description.Description;
                    break;
                }
            }
            StartCoroutine(AppearAnimation());

        }
        
        private IEnumerator AppearAnimation()
        {
            _canvasGroup.alpha = 0f;
            float elapsed = 0f;

            while (elapsed < _animationDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / _animationDuration;
                _canvasGroup.alpha = _appearCurve.Evaluate(t);
                yield return null;
            }

            _canvasGroup.alpha = 1f;
        }
    }

    [Serializable]
    public class EndOfTheGameResultDescription
    {
        public GameResult GameResult;
        public string Description;
        public string Title;
    }

    public enum GameResult
    {
        Win,
        LoseNoFood,
        LoseLighthouseLightWentOut
    }
}