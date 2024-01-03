using A2DK.Phys;
using UnityEngine;

namespace Mechanics {
    public class Wall : Solid, IGrappleAble {
        public override bool Collidable() {
            return true;
        }

        public override bool PlayerCollide(Actor p, Vector2 direction) {
            // if (direction.y > 0) {
            //     p.BonkHead();
            // }
            return true;
        }

        public (Vector2 curPoint, IGrappleAble attachedTo) GetGrapplePoint(Actor p, Vector2 rayCastHit) => (rayCastHit, this);
        public Vector2 ContinuousGrapplePos(Vector2 origPos) => origPos;

        public PhysObj GetPhysObj() => this;
    }
}