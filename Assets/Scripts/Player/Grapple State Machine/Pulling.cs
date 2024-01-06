using System.Data;
using A2DK.Phys;
using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class GrappleStateMachine
    {
        
        public class Pulling : GrappleState
        {
            
            // private PhysObj _attachedTo;
            // private Vector2 _prevV;

            public override void Enter(GrappleStateInput i)
            {
                
                
                // smActor.StartGrapple(Input.CurrentGrapplePos);
            }

            public override void FixedUpdate()
            {
                Input.CurrentGrapplePos = Input.AttachedTo.ContinuousGrapplePos(Input.CurrentGrapplePos, MySM.MyPhysObj);
                base.FixedUpdate();
                // if (_attachedTo.velocity == Vector2.zero && _prevV != Vector2.zero)
                // {
                //     smActor.ApplyVelocity(_prevV);
                // }
                // _prevV = _attachedTo.velocity;
                // GameTimer.FixedUpdate(_grappleTimer);
            }

            /*public override void CollideHorizontal() {
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
            }*/

            public override void GrappleFinished()
            {
                MySM.Transition<Idle>();
                MyCore.MovementStateMachine.RefreshAbilities();
            }
            
            /*
            

            public override Vector2 ResolveRide(Vector2 direction)
            {
                Input.CurrentGrapplePos = Input.AttachedTo.ContinuousGrapplePos(Input.CurrentGrapplePos);

                if (Input.AttachedTo == null) return direction;
                bool atMovingTowards = AttachedMovingTowards();

                if (atMovingTowards) return Vector2.zero;
                return direction;
            }

            public override PhysObj ResolveRidingOn(PhysObj p) => Input.AttachedToPhysObj;

            public override void Push(Vector2 direction, Solid pusher)
            {
                if (pusher == Input.AttachedToPhysObj)
                {
                    Input.CurrentGrapplePos = Input.AttachedTo.ContinuousGrapplePos(Input.CurrentGrapplePos);
                }
            }*/
        }
    }
}