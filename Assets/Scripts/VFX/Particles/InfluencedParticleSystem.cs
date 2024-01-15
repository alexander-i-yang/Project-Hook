using UnityEngine;

namespace VFX
{
    [RequireComponent(typeof(ParticleSystem))]
    public class InfluencedParticleSystem : ParticleLauncher
    {
        private ParticleSystem _psystem;
        [SerializeField] private int particleCount;
        [SerializeField] private float influenceWeight;

        private void Awake()
        {
            _psystem = GetComponent<ParticleSystem>();
        }
    
        private void ApplyParticleVelocity(Vector2 influencedV)
        {
            ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_psystem.particleCount+1];
            _psystem.GetParticles(particles);
            for (int i = 0; i < particles.Length; ++i)
            {
                particles[i].velocity += (Vector3) influencedV * influenceWeight;
                // particles[i].velocity = new Vector3(1000, 1000);
            }
            _psystem.SetParticles(particles, particles.Length);
        }

        public override void Launch(Vector2 v, Vector2 position)
        {
            if (_psystem == null) Awake();
            _psystem.Emit(particleCount);
            ApplyParticleVelocity(v);
        }
    }
}