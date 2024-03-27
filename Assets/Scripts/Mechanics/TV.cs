using System.Collections;
using ASK.Helpers;
using Combat;
using UnityEngine;
using UnityEngine.Events;

namespace Mechanics
{
    public class TV : MonoBehaviour, IPunchable
    {
        private bool _broken;

        [SerializeField] private UnityEvent<Vector2> onPunch;
        
        public bool ReceivePunch(Vector2 v)
        {
            if (_broken) return false;
            _broken = true;
            GetComponent<SpriteRenderer>().enabled = false;
            onPunch.Invoke(v);
            return true;
        }

        void Start()
        {
            // Vector2 v = new Vector2(10000, 10000);
            // StartCoroutine(Helper.DelayAction(2, () => ReceivePunch(v)));
        }
    }
}