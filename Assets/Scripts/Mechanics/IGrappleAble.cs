using A2DK.Phys;
using UnityEngine;

namespace Mechanics {
    public interface IGrappleAble {
        public (Vector2 curPoint, PhysObj attachedTo) GetGrapplePoint(Actor p, Vector2 rayCastHit);
    }
}