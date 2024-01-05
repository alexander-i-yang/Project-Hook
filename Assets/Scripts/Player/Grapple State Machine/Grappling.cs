using System.Data;
using A2DK.Phys;
using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class GrappleStateMachine
    {
        
        public class Grappling : GrappleState
        {
            
            // private PhysObj _attachedTo;
            // private Vector2 _prevV;

            public override void Enter(GrappleStateInput i)
            {
                
                
                // _grappleTimer = GameTimer.StartNewTimer(core.GrappleWarmTime);
                // _prevV = Vector2.zero;
                smActor.StartGrapple(Input.CurrentGrapplePos);
                // _attachedTo.GrappleRider = smActor;
            }

            public override void FixedUpdate()
            {
                smActor.GrappleUpdate(Input.CurrentGrapplePos, 0);
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
                    smActor.CollideHorizontalGrapple();
                    MySM.Transition<Idle>();
                }
            }

            public override Vector2 ProcessCollideHorizontal(Vector2 oldV, Vector2 newV) {
                if (MyCore.GrappleCollideWallStop)
                {
                    MySM.Transition<Idle>();
                    return smActor.CollideHorizontalGrapple();
                }

                return newV;
            }   

            public override void CollideVertical() {
                smActor.CollideVerticalGrapple();
                if (MyCore.GrappleCollideWallStop) MySM.Transition<Idle>();
            }

            public override void GrappleFinished()
            {
                base.GrappleFinished();
                smActor.GrappleBoost(Input.CurrentGrapplePos);
                MySM.Transition<Idle>();
                MyCore.MovementStateMachine.RefreshAbilities();
            }

            public override Vector2 MoveX(PlayerActor p, Vector2 velocity, int direction)
            {
                // velocity = smActor.CalcMovementX(0, core.MaxAirAcceleration, core.AirResistance);
                velocity = smActor.MoveXGrapple(velocity, Input.CurrentGrapplePos, direction);
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
                Vector2 atDisplacement = at.transform.position - smActor.transform.position;
                return Vector2.Dot(atV, atDisplacement) <= 0;
            }

            public override Vector2 ResolveRide(Vector2 direction)
            {
                Input.CurrentGrapplePos = Input.AttachedTo.ContinuousGrapplePos(Input.CurrentGrapplePos);

                if (Input.AttachedTo == null) return direction;
                bool atMovingTowards = AttachedMovingTowards();
                
                if (atMovingTowards) return Vector2.zero;
                return direction;
            }

            public override PhysObj ResolveRidingOn(PhysObj p) => Input.AttachedToPhysObj;
        }
    }
}