using System;
using Mechanics;
using UnityEngine;

namespace Player
{
    public class StickyCollider : MonoBehaviour
    {
        private Collider2D _stickyCollider;

        private void Awake()
        {
            _stickyCollider = GetComponent<Collider2D>();
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