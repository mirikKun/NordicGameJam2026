using Project.Scripts.Grid;
using UnityEngine;
using UnityEngine.UI;

namespace Project.Scripts.Animation
{
    public class LightHouseIndicator:MonoBehaviour
    {
        [SerializeField] private Image _lightHouseIndicatorImage;
        [SerializeField] private RectTransform _root;

        [SerializeField] private float _flickeringDuration;
        [SerializeField] private AnimationCurve _flickeringPower;
        [SerializeField] private AnimationCurve _flickeringPulse;

        private float elapsedTime=0;
        public void Tick(float lighthouseProgress)
        {
            elapsedTime+=Time.deltaTime;
            _lightHouseIndicatorImage.fillAmount = lighthouseProgress;
            float progress = _flickeringPulse.Evaluate((elapsedTime % _flickeringDuration)/_flickeringDuration);
            float scale =1+ progress * _flickeringPower.Evaluate(1-lighthouseProgress);
            _root.localScale=scale*Vector3.one;
        }
    }
}