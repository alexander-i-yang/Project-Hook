using System.Data;
using A2DK.Phys;
using ASK.Core;
using UnityEngine;

namespace Mechanics
{
    public partial class GrapplerStateMachine
    {
        
        public class Swinging : GrappleState
        {
            public override void Enter(GrappleStateInput i)
            {
                
                
                MySM.StartGrapple(Input.CurrentGrapplePos);
            }

            public override void FixedUpdate()
            {
                var oldGpos = Input.CurrentGrapplePos;
                Input.CurrentGrapplePos = Input.AttachedTo.ContinuousGrapplePos(Input.CurrentGrapplePos, MySM.MyPhysObj);
                var newGPos = Input.CurrentGrapplePos;
                MySM.GrappleUpdate(Input.CurrentGrapplePos, 0);
            }

            public override void CollideHorizontal() {
                if (MySM.GrappleCollideWallStop)
                {
                    MySM.CollideHorizontalGrapple();
                    MySM.Transition<Idle>();
                }
            }

            public override Vector2 ProcessCollideHorizontal(Vector2 oldV, Vector2 newV) {
                if (MySM.GrappleCollideWallStop)
                {
                    MySM.Transition<Idle>();
                    return MySM.CollideHorizontalGrapple();
                }

                return newV;
            }   

            public override void CollideVertical() {
                MySM.CollideVerticalGrapple();
                if (MySM.GrappleCollideWallStop) MySM.Transition<Idle>();
            }

            public override void GrappleFinished()
            {
                Input.AttachedTo.DetachGrapple();
                MySM.GrappleBoost();
                MySM.Transition<Idle>();
                MySM.OnGrappleDetach?.Invoke();
                // MyCore.MovementStateMachine.RefreshAbilities();
            }

            public override Vector2 MoveX(Vector2 velocity, int direction)
            {
                // velocity = smActor.CalcMovementX(0, core.MaxAirAcceleration, core.AirResistance);
                velocity = MySM.MoveXGrapple(velocity, Input.CurrentGrapplePos, direction);
                return velocity;
            }

            /**
             * Returns true when AttachedTo is moving towards the player.
             * Constraint: AttachedTo cannot be null.
             */
            private bool AttachedMovingTowards()
            {
                var at = Input.AttachedToPhysObj;
                if (at == null) throw new ConstraintException("AttachedTo must not be null");
                Vector2 atV = at.velocity;
                Vector2 atDisplacement = at.transform.position - MySM.MyPhysObj.transform.position;
                return Vector2.Dot(atV, atDisplacement) <= 0;
            }

            public override Vector2 ResolveRide(Vector2 direction)
            {
                if (Input.AttachedTo == null) return direction;
                
                if (AttachedMovingTowards() && direction.y >= 0) return Vector2.zero;
                return direction;
            }

            public override PhysObj ResolveRidingOn(PhysObj p) => Input.AttachedToPhysObj;

            public override void Push(Vector2 direction, PhysObj pusher)
            {
                if (pusher == Input.AttachedToPhysObj)
                {
                    Input.CurrentGrapplePos = Input.AttachedTo.ContinuousGrapplePos(Input.CurrentGrapplePos, MySM.MyPhysObj);
                }
            }

            public override bool ShouldApplyV()
            {
                return AttachedMovingTowards();
            }
        }
    }
}