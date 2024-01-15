using System.Collections.Generic;
using UnityEngine;

namespace VFX
{
    public class CloneParticleLauncher : PrefabParticleLauncher
    {
        [SerializeField] private GameObject _particlePrefab;
        [SerializeField] private int _numParticles;
        public override GameObject[] GetParticles()
        {
            var ret = new List<GameObject>();
            for (int i = 0; i < _numParticles; ++i)
            {
                ret.Add(_particlePrefab);
            }
            return ret.ToArray();
        }
    }
}