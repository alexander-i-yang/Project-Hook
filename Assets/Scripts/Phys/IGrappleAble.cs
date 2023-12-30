using UnityEngine;

namespace A2DK.Phys {
    public interface IGrappleAble {
        public (Vector2 curPoint, bool hit) GetGrapplePoint(Actor p, Vector2 rayCastHit);
    }
}