using System;
using System.Collections;
using UnityEngine;

namespace Project.Scripts.Animation
{
    public class FadeAnimationOnStart:MonoBehaviour
    {
        [SerializeField] private float _animationDuration;
        [SerializeField] private AnimationCurve _animationCurve;
        [SerializeField] private CanvasGroup _canvasGroup;
        private IEnumerator Start()
        {
            float elapsedTime = 0;
            while (elapsedTime < _animationDuration)
            {
                
                elapsedTime += Time.deltaTime;
                _canvasGroup.alpha =_animationCurve.Evaluate(elapsedTime / _animationDuration) ;
                yield return null;
            }
        }
    }
}