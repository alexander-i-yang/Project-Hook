using System;
using A2DK.Phys;
using UnityEngine;

namespace Combat
{
    
    [RequireComponent(typeof(Actor))]
    public class GoombaBehavior : MonoBehaviour, IBehavior
    {
        private IControllable _controllable;
        
        private void Awake()
        {
            _controllable = GetComponent<IControllable>();
        }

        public void MoveDirection(int direction)
        {
            _controllable.Walk(direction);
        }
    }

    public interface IControllable
    {
        public void Walk(int direction);
    }
}