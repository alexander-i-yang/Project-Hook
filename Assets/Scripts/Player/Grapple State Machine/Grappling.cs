using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class GrappleStateMachine
    {
        public class Grappling : GrappleState
        {

            public override void Enter(GrappleStateInput i)
            {
                // _grappleTimer = GameTimer.StartNewTimer(core.GrappleWarmTime);
                smActor.StartGrapple(Input.currentGrapplePos);
            }

            public override void FixedUpdate()
            {
                smActor.GrappleUpdate(Input.currentGrapplePos, 0);
                // GameTimer.FixedUpdate(_grappleTimer);
            }

            public override void CollideHorizontal() {
                smActor.CollideHorizontalGrapple();
                MySM.Transition<Idle>();
            }

            public override void CollideVertical() {
                smActor.CollideVerticalGrapple();
                MySM.Transition<Idle>();
            }

            public override void GrappleFinished()
            {
                base.GrappleFinished();
                smActor.GrappleBoost(Input.currentGrapplePos);
                MySM.Transition<Idle>();
            }

            public override Vector2 MoveX(PlayerActor p, Vector2 velocity, int direction) {
                // velocity = smActor.CalcMovementX(0, core.MaxAirAcceleration, core.AirResistance);
                velocity = smActor.MoveXGrapple(velocity, Input.currentGrapplePos, direction);
                return velocity;
            }
        }
    }
}