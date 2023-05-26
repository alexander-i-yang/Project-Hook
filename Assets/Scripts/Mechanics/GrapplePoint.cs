using A2DK.Phys;
using UnityEngine;

namespace Mechanics {
    public class GrapplePoint : Solid {
        public override bool Collidable() {
            return false;
        }

        public override bool PlayerCollide(PlayerActor p, Vector2 direction) {
            // if (direction.y > 0) {
            //     p.BonkHead();
            // }
            return false;
        }
    }
}