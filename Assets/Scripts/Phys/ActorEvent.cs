using System;
using UnityEngine;
using UnityEngine.Events;

namespace A2DK.Phys
{
    [Serializable]
    public class ActorEvent : UnityEvent<Vector2>
    {
        [SerializeField]
        ActorEvent pEvent;

        public void OnEventRaised(Vector2 pos)
        {
            pEvent.Invoke(pos);
        }
    }
}