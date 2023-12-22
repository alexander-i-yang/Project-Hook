using A2DK.Phys;
using UnityEngine;

namespace Player {
    public class GrappleHook : Solid {
        public bool DidCollide {get; private set;} //TODO think of a better way to do this. WOW is it bad practice.
        
        public override bool Collidable() {
            return false;
        }

        public override bool PlayerCollide(Actor p, Vector2 direction) {
            return false;
        }

        public override bool OnCollide(PhysObj p, Vector2 v) {
            if (p.Collidable()) {DidCollide = true; return true;}
            return false;
        }

        public override bool Squish(PhysObj p, Vector2 v) {
            return false;
        }

        public void SetVelocity(Vector2 v) {
            velocity = v;
        }

        public bool SetPos(Vector2 v) {
            transform.position = v;
            return CheckCollisions(Vector2.zero, (p, d) => {
                return p.IsGround(this);
            });
        }

        public void Reset(Vector3 resetPos) {
            transform.localPosition = resetPos;
            velocity = Vector2.zero;
            DidCollide = false;
        }

    }
}