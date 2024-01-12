using System;
using System.Collections.Generic;
using ASK.Core;
using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VFX
{
    public class ParticleBreaker : MonoBehaviour
    {
        private BrokenParticle[] _originalParticles;

        private Material _whiteMaterial;

        // private PlayerActor _actor;
        // private PlayerSpawnManager _spawnManager;
        // private InfluencedParticleSystem _smokeParticles;
        // private InfluencedParticleSystem _sparkParticles;

        [SerializeField] private float deathParticleInheritVWeight = 1f;
        [SerializeField] private float deathParticlePersistTime = 1f;
        [SerializeField] private float deathParticleFadeTime = 1f;

        [MinMaxRange(0, 200), SerializeField] private RangedInt velocityRange = new(100, 200);

        [MinMaxRange(0, 500), SerializeField] private RangedInt rotationVRange = new(10, 50);

        // private BrokenParticle _deathParticlePool;

        void Awake()
        {
            _originalParticles = GetComponentsInChildren<BrokenParticle>();
            // _actor = GetComponentInParent<PlayerActor>();

            // InfluencedParticleSystem[] psystems = GetComponentsInChildren<InfluencedParticleSystem>();
            // _smokeParticles = psystems[0];
            // _sparkParticles = psystems[1];

            foreach (var part in _originalParticles)
            {
                part.gameObject.SetActive(false);
            }
        }

        void OnEnable()
        {
            // _spawnManager = GetComponentInParent<PlayerSpawnManager>();
            // _spawnManager.OnPlayerRespawn += OnRespawn;
        }

        void OnDisable()
        {
            // _spawnManager.OnPlayerRespawn -= OnRespawn;
        }

        public void Launch(Vector2 actorV)
        {
            foreach (var part in _originalParticles)
            {
                float angle = Random.Range(0, 360);
                float magnitude = Random.Range(velocityRange.Min, velocityRange.Max);
                Vector2 v = new Vector2((float)(Math.Cos(angle) * magnitude),
                    (float)(Math.Sin(angle) * magnitude));

                float rotationalV = Random.Range(rotationVRange.Min, rotationVRange.Max);
                if (Random.value > 0.5) rotationalV *= -1;

                BrokenParticle newPart = Instantiate(part.gameObject).GetComponent<BrokenParticle>();
                Game.ParticlePool.ReceiveParticle(newPart.transform);
                newPart.transform.position = part.transform.position;
                newPart.gameObject.SetActive(true);
                newPart.Init();
                newPart.Launch(
                    v + actorV * deathParticleInheritVWeight,
                    rotationalV,
                    deathParticlePersistTime,
                    deathParticleFadeTime
                );
            }
        }

        /*private void DeadStop()
        {
            _actor.DeadStop();
        }*/

        //Called in Unity Animator - Do not delete
        /*#region AnimatorEvents

        public void TriggerParticles()
        {
            Vector2 appliedV = _actor.velocity;
            _smokeParticles.Emit(appliedV);
            _sparkParticles.Emit(appliedV);
            DeadStop();
            Launch(appliedV);
        }

        public void DeathRecoil()
        {
            _actor.DeathRecoil();
        }

        public void Respawn()
        {
            // _spawnManager.OnPlayerRespawn += OnRespawn;
            _spawnManager.Respawn();
        }
        #endregion*/
    }
}