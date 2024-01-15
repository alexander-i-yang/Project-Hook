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
    }
}