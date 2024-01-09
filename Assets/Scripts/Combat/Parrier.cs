using System;
using System.Collections.Generic;
using UnityEngine;

namespace Combat
{
    public class Parrier : MonoBehaviour
    {
        private Collider2D _myCollider;

        private HashSet<IPunchable> _curPunchables = new();

        [SerializeField] private float _punchV;

        private void Awake()
        {
            _myCollider = GetComponent<Collider2D>();
        }

        public void SetAim(Vector2 aimPos)
        {
            float a = Vector2.SignedAngle(Vector2.right, aimPos - (Vector2)transform.position);
            Quaternion q = transform.rotation;
            q.eulerAngles = new Vector3(0, 0, a);
            transform.rotation = q;
        }

        public void Parry(Vector2 aimPos)
        {
            Vector2 v = (aimPos - (Vector2)transform.position).normalized * _punchV;
            foreach (var p in _curPunchables)
            {
                p.ReceivePunch(v);
            }
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