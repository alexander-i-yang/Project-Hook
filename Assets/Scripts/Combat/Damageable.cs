using System;
using ASK.Core;
using UnityEngine;

namespace Combat
{
    public class Damageable : MonoBehaviour
    {
        public Action<Vector2> OnDamaged;

        [SerializeField] private float iFrameTime;
        private float _curIFrameTime;
        
        private void FixedUpdate()
        {
            if (_curIFrameTime > 0) _curIFrameTime -= Game.TimeManager.FixedDeltaTime;
        }

        public void TakeDamage(Vector2 enemyPos)
        {
            if (_curIFrameTime <= 0)
            {
                _curIFrameTime = iFrameTime;
                OnDamaged?.Invoke(enemyPos);
            }
        }
    }
}