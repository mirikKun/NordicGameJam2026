using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Animation
{
    public class TextAnimation:MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _icon;
        [SerializeField] private float _duration;

        [SerializeField] private AnimationCurve _moveCurve;
        [SerializeField] private AnimationCurve _fadeCurve;

        public void Setup(string text, Sprite icon, Color color, Vector3 from, Vector3 to)
        {
            _text.text = text;
            _icon.sprite = icon;
            _icon.color = color;
            _text.color = color;
            transform.position = from;
            StartCoroutine(Animate(from, to));
        }

        private IEnumerator Animate(Vector3 from, Vector3 to)
        {
            float elapsed = 0f;

            while (elapsed < _duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / _duration;
                transform.position = Vector3.Lerp(from, to, _moveCurve.Evaluate(t));
                _text.alpha = _fadeCurve.Evaluate(t);
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}