using ASK.Core;
using MyBox;
using UnityEngine;

namespace VFX
{
    public abstract class PrefabParticleLauncher : ParticleLauncher
    {
        
        [MinMaxRange(0, 200), SerializeField] protected RangedInt velocityRange = new(100, 200);
        [MinMaxRange(0, 500), SerializeField] protected RangedInt rotationVRange = new(10, 50);
        [SerializeField] private float inheritVWeight = 1f;

        public abstract GameObject[] GetParticles();

        public void InstantiateParticle(GameObject part, Vector2 actorV, Vector2 position)
        {
            float angle = Random.Range(0, 360);
            float magnitude = Random.Range(velocityRange.Min, velocityRange.Max);
            Vector2 v = new Vector2((float)(Mathf.Cos(angle) * magnitude),
                (float)(Mathf.Sin(angle) * magnitude));

            float rotationalV = Random.Range(rotationVRange.Min, rotationVRange.Max);
            if (Random.value > 0.5) rotationalV *= -1;

            PrefabParticle newPart = Instantiate(part).GetComponent<PrefabParticle>();
            newPart.transform.position = position;
            newPart.gameObject.SetActive(true);
            newPart.Launch(v + actorV * inheritVWeight, rotationalV);
            newPart.GetComponent<SpriteRenderer>().enabled = true;
            Game.ParticlePool.ReceiveParticle(newPart.transform);
        }

        public override void Launch(Vector2 actorV, Vector2 position)
        {
            foreach (var part in GetParticles())
            {
                InstantiateParticle(part, actorV, position);
            }
        }
    }
}