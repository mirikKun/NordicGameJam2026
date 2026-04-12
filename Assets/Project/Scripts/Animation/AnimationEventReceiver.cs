using System;
using UnityEngine;

namespace Project.Scripts.Animation
{
    public class AnimationEventReceiver:MonoBehaviour
    {
        public event Action Called;
        public void Call()
        {
            Called?.Invoke();
        }
    }
}