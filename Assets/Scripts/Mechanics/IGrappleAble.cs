using A2DK.Phys;
using UnityEngine;

namespace Mechanics {
    public interface IGrappleAble {
        public (Vector2 curPoint, IGrappleAble attachedTo) GetGrapplePoint(Actor p, Vector2 rayCastHit);

        /**
         * Called every frame. Returns new position of grapple regardless of attached PhysObj movement.
         */
        public Vector2 ContinuousGrapplePos(Vector2 origPos);

        public PhysObj GetPhysObj();
    }
}