using System;
using A2DK.Phys;
using UnityEngine;

namespace Combat
{
    
    [RequireComponent(typeof(IControllable))]
    public class GoombaBehavior : MonoBehaviour, IBehavior
    {
        private IControllable _controllable;
        
        private void Awake()
        {
            _controllable = GetComponent<IControllable>();
        }
    }

    public interface IControllable
    {
        public void Walk(int direction);
    }
}