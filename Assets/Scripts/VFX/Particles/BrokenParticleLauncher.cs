using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VFX
{
    public class BrokenParticleLauncher : PrefabParticleLauncher
    {
        private GameObject[] _originalParticles;
        
        void Awake()
        {
            _originalParticles = new List<PrefabParticle>(GetComponentsInChildren<PrefabParticle>()).Select(p => p.gameObject).ToArray();
            
            foreach (var part in _originalParticles)
            {
                part.gameObject.SetActive(false);
            }
        }

        public override GameObject[] GetParticles() => _originalParticles;
        
        public override void Launch(Vector2 actorV, Vector2 position)
        {
            foreach (var part in GetParticles())
            {
                InstantiateParticle(part, actorV, part.transform.position);
            }
        }

        public void Launch(Vector2 actorV)
        {
            Launch(actorV, transform.position);
        }
    }
}