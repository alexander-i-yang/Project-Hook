using System;
using ASK.Core;
using UnityEngine;
using Helpers;

namespace Player
{
    [RequireComponent(typeof(PlayerCore))]
    public class PlayerDeathManager : MonoBehaviour
    {
        private PlayerCore _core;

        // private GameTimer2 _deathTimer;
        
        public event Action OnPlayerRespawn;
        public event Action OnDeath;

        void Awake()
        {
            _core = GetComponent<PlayerCore>();
        }

        public void Die()
        {
            OnDeath?.Invoke();
            GameTimerManager.Instance.StartTimer(_core.DeathTime, Respawn, IncrementType.FIXED_UPDATE);
        }

        public void Respawn()
        {
            print("Respawn");
            OnPlayerRespawn?.Invoke();
        }
    }
}