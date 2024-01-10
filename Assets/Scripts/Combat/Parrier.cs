using System;
using System.Collections.Generic;
using A2DK.Phys;
using UnityEngine;
using UnityEngine.Events;

namespace Combat
{
    public class Parrier : MonoBehaviour
    {
        private Collider2D _myCollider;

        private HashSet<IPunchable> _curPunchables = new();

        [SerializeField] private float _punchV;

        [SerializeField] private UnityEvent OnAim;
        [SerializeField] private UnityEvent OnPunch;
        [SerializeField] private UnityEvent OnIdle;

        private void Awake()
        {
            _myCollider = GetComponentInChildren<Collider2D>();
        }

        public void SetAim(Vector2 aimPos)
        {
            float a = Vector2.SignedAngle(Vector2.right, aimPos - (Vector2)transform.position);
            Quaternion q = transform.rotation;
            q.eulerAngles = new Vector3(0, 0, a);
            transform.rotation = q;
            OnAim?.Invoke();
        }

        public void Parry(Vector2 aimPos, Vector2 velocity)
        {
            Vector2 v = (aimPos - (Vector2)transform.position).normalized * _punchV;
            v = v.normalized * (v.magnitude + velocity.magnitude);
            foreach (var p in _curPunchables)
            {
                p.ReceivePunch(v);
            }
            OnPunch?.Invoke();
        }

        public void Idle()
        {
            OnIdle?.Invoke();
        }

        private void FixedUpdate()
        {
            Vector2 v = GetComponentInParent<Actor>().velocity;
            // _myCollider.transform.localScale = Vector3.one * Mathf.Min(Mathf.Sqrt(v.magnitude));
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            var punchable = other.GetComponent<IPunchable>();
            if (punchable != null)
            {
                _curPunchables.Add(punchable);
            }
        }
        
        void OnTriggerExit2D(Collider2D other)
        {
            var punchable = other.GetComponent<IPunchable>();
            if (punchable != null)
            {
                _curPunchables.Remove(punchable);
            }
        }
    }
}