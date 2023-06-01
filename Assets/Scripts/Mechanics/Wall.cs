using A2DK.Phys;
using UnityEngine;

namespace Mechanics {
    public class Wall : Solid, IGrappleAble {
        public override bool Collidable() {
            return true;
        }

        public override bool PlayerCollide(PlayerActor p, Vector2 direction) {
            // if (direction.y > 0) {
            //     p.BonkHead();
            // }
            return true;
        }
    }
}