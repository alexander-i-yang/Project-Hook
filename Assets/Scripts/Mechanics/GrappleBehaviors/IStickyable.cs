using System;
using A2DK.Phys;
using UnityEngine;

namespace Mechanics {
    public interface IStickyable
    {
        public void OnStickyEnter(Collider2D stickyCollider);
        public void OnStickyExit(Collider2D stickyCollider);
    }
}