using ASK.Helpers;
using UnityEngine;

namespace VFX
{
    public class DelayLauncher : MonoBehaviour
    {
        [SerializeField] private Vector2 launchV;
        
        [SerializeField] private float delay;

        private ParticleLauncher _particleLauncher;

        void Awake()
        {
            _particleLauncher = GetComponent<ParticleLauncher>();
        }

        public void Launch()
        {
            StartCoroutine(Helper.DelayAction(delay, () =>
            {
                _particleLauncher.Launch(launchV, _particleLauncher.transform.position);
            }));
        }
    }
}