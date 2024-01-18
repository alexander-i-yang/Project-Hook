using System;
using Mechanics;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class StickyCollider : MonoBehaviour
    {
        private Collider2D _stickyCollider;
        private PlayerInputController _input;
        [SerializeField] private float distance;

        [SerializeField] private UnityEvent firstSticky;

        private bool hasFirstSticky;
        
        private void Awake()
        {
            _stickyCollider = GetComponent<Collider2D>();
            _input = GetComponentInParent<PlayerInputController>();
        }

        private void FixedUpdate()
        {
            var p = (Vector2)_input.transform.position;
            var v = _input.GetAimPos(p) - p;
            transform.position = v.normalized * distance + p;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var pullable = other.GetComponent<IStickyable>();
            pullable?.OnStickyEnter(_stickyCollider);
            if (!hasFirstSticky && other.GetComponent<Crate>() != null)
            {
                firstSticky.Invoke();
                hasFirstSticky = true;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            var pullable = other.GetComponent<IStickyable>();
            pullable?.OnStickyExit(_stickyCollider);
        }
    }
}