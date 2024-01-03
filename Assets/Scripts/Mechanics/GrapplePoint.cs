using A2DK.Phys;
using UnityEngine;
using System.Collections.Generic;

namespace Mechanics {
    public class GrapplePoint : Solid, IGrappleAble {
        public override bool Collidable() {
            return false;
        }

        public override bool PlayerCollide(Actor p, Vector2 direction) {
            // if (direction.y > 0) {
            //     p.BonkHead();
            // }
            return false;
        }

        public (Vector2 curPoint, IGrappleAble attachedTo) GetGrapplePoint(Actor p, Vector2 rayCastHit) {
            bool pOverlap = false;
            
            // Collider2D pCollider = p.GetComponent<Collider2D>();
            // List<Collider2D> overlapResults = new();
            // ContactFilter2D filter = new ContactFilter2D();
            // filter.SetLayerMask(LayerMask.NameToLayer("Interactable"));
            // Physics2D.OverlapCollider(myCollider, filter, overlapResults); //WARNING: mycollider checks against last phys update, not current
            
            // foreach(var c in overlapResults) {
            //     if (c == pCollider) {
            //         pOverlap = true;
            //         break;
            //     }
            // }
            
            // var ret = (
            //     curPoint: transform.position,
            //     attachedTo: pOverlap ? null : this
            // );
            return (transform.position, this);
        }

        public Vector2 ContinuousGrapplePos(Vector2 origPos) => transform.position;

        public PhysObj GetPhysObj() => this;
    }
}