using System;
using UnityEngine;
using UnityEngine.Events;

namespace A2DK.Phys
{
    [Serializable]
    public class ActorEvent : UnityEvent<Vector3>
    {
        [SerializeField]
        ActorEvent pEvent;

        public void OnEventRaised(Vector3 pos)
        {
            pEvent.Invoke(pos);
        }
    }
}