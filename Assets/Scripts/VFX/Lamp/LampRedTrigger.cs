using ASK.Helpers;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace VFX
{
    public class LampRedTrigger : MonoBehaviour
    {
        private Lamp[] _lamps;

        [SerializeField] private float delay;
        
        void Awake()
        {
            _lamps = FindObjectsOfType<Lamp>();
        }

        public void Trigger()
        {
            StartCoroutine(Helper.DelayAction(delay, () =>
            {
                foreach (var lamp in _lamps)
                {
                    lamp.TurnRed();
                }
            }));
        }
    }
}