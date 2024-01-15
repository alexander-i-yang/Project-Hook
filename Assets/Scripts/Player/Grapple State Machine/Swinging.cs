using System.Data;
using A2DK.Phys;
using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class GrappleStateMachine
    {
        
        public class Swinging : GrappleState
        {
            
            // private PhysObj _attachedTo;
            // private Vector2 _prevV;

            public override void Enter(GrappleStateInput i)
            {
                
                
                // _grappleTimer = GameTimer.StartNewTimer(core.GrappleWarmTime);
                // _prevV = Vector2.zero;
                MySM.MyPhysObj.StartGrapple(Input.CurrentGrapplePos);
                // _attachedTo.GrappleRider = smActor;
            }

            public override void FixedUpdate()
            {
                Input.CurrentGrapplePos = Input.AttachedTo.ContinuousGrapplePos(Input.CurrentGrapplePos, MySM.MyPhysObj);
                MySM.MyPhysObj.GrappleUpdate(Input.CurrentGrapplePos, 0);
                // if (_attachedTo.velocity == Vector2.zero && _prevV != Vector2.zero)
                // {
                //     smActor.ApplyVelocity(_prevV);
                // }
                // _prevV = _attachedTo.velocity;
                // GameTimer.FixedUpdate(_grappleTimer);
            }

            public override void CollideHorizontal() {
                if (MyCore.GrappleCollideWallStop)
                {
                    MySM.MyPhysObj.CollideHorizontalGrapple();
                    MySM.Transition<Idle>();
                }
            }

            public override Vector2 ProcessCollideHorizontal(Vector2 oldV, Vector2 newV) {
                if (MyCore.GrappleCollideWallStop)
                {
                    MySM.Transition<Idle>();
                    return MySM.MyPhysObj.CollideHorizontalGrapple();
                }

                return newV;
            }   

            public override void CollideVertical() {
                MySM.MyPhysObj.CollideVerticalGrapple();
                if (MyCore.GrappleCollideWallStop) MySM.Transition<Idle>();
            }

            public override void GrappleFinished()
            {
                Input.AttachedTo.DetachGrapple();
                MySM.MyPhysObj.GrappleBoost(Input.CurrentGrapplePos);
                MySM.Transition<Idle>();
                MyCore.MovementStateMachine.RefreshAbilities();
            }

            public override Vector2 MoveX(Vector2 velocity, int direction)
            {
                // velocity = smActor.CalcMovementX(0, core.MaxAirAcceleration, core.AirResistance);
                velocity = MySM.MyPhysObj.MoveXGrapple(velocity, Input.CurrentGrapplePos, direction);
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