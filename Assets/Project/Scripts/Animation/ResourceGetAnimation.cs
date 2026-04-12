using TMPro;
using UnityEngine;

namespace Project.Scripts.Animation
{
    public class ResourceGetAnimation : MonoBehaviour
    {
        [SerializeField] private TextAnimation _textPrefab;
        [SerializeField] private Transform _from;
        [SerializeField] private Transform _to;
        [SerializeField] private Vector3 _randomStartOffset;
        [SerializeField] private Vector3 _randomEndOffset;

        public void PlayAnimation(string text,Sprite icon, Color color)
        {
            Vector3 startOffset = new Vector3(
                Random.Range(-_randomStartOffset.x, _randomStartOffset.x),
                Random.Range(-_randomStartOffset.y, _randomStartOffset.y),
                Random.Range(-_randomStartOffset.z, _randomStartOffset.z)
            );

            Vector3 endOffset = new Vector3(
                Random.Range(-_randomEndOffset.x, _randomEndOffset.x),
                Random.Range(-_randomEndOffset.y, _randomEndOffset.y),
                Random.Range(-_randomEndOffset.z, _randomEndOffset.z)
            );

            Vector3 from = _from.position + startOffset;
            Vector3 to = from;
            to.y = _to.position.y;
            to = to + endOffset;

            TextAnimation textAnimation = Instantiate(_textPrefab, from, Quaternion.identity, transform);
            textAnimation.transform.localEulerAngles = Vector3.zero;
            textAnimation.Setup(text,icon, color, from, to);
        }
    }
}