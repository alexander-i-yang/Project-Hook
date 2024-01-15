using System;
using Mechanics;
using UnityEngine;

namespace Player
{
    public class StickyCollider : MonoBehaviour
    {
        private Collider2D _stickyCollider;
        private PlayerInputController _input;
        [SerializeField] private float distance;
        
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
            var pullable = other.GetComponent<IPullable>();
            pullable?.OnStickyEnter(_stickyCollider);
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            var pullable = other.GetComponent<IPullable>();
            pullable?.OnStickyExit(_stickyCollider);
        }
    }
}