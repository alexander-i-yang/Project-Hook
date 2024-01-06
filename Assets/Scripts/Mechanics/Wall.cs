using A2DK.Phys;
using UnityEngine;

namespace Mechanics {
    public class Wall : Solid, IGrappleAble {
        public override bool Collidable(PhysObj collideWith) {
            return true;
        }

        public (Vector2 curPoint, IGrappleAble attachedTo) GetGrapplePoint(Actor p, Vector2 rayCastHit) => (rayCastHit, this);
        public Vector2 ContinuousGrapplePos(Vector2 origPos, Actor grapplingActor) => origPos;

        public PhysObj GetPhysObj() => this;
        public GrappleapleType GrappleapleType() => Mechanics.GrappleapleType.SWING;
    }
}