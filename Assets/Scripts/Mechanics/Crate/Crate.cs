using A2DK.Phys;
using UnityEngine;

namespace Mechanics {
    public class Crate : Actor, IGrappleAble {
        public override bool Collidable() {
            return true;
        }

        public override bool PlayerCollide(Actor p, Vector2 direction) {
            // if (direction.y > 0) {
            //     p.BonkHead();
            // }
            return true;
        }

        public override bool Squish(PhysObj p, Vector2 d)
        {
            Destroy(gameObject);
            return false;
        }

        public (Vector2 curPoint, IGrappleAble attachedTo) GetGrapplePoint(Actor p, Vector2 rayCastHit)
        {
            velocity = p.transform.position - transform.position;
            return (transform.position, this);
        }

        public Vector2 ContinuousGrapplePos(Vector2 origPos) => transform.position;

        public PhysObj GetPhysObj() => this;

        private void FixedUpdate()
        {
            Fall();
            ApplyVelocity(ResolveJostle());
            MoveTick();
        }
    }
}