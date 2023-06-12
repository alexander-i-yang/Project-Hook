using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public partial class MovementStateMachine
    {
        public class Diving : MovementState
        {
            public override void Enter(MovementStateInput i)
            {
                // PlayAnimation(PlayerAnimations.DIVING);
                smActor.Dive();
                Input.canDive = false;
                Input.canJumpCut = false;
                
                // var divePEmission = MySM._diveParticles.emission;
                // divePEmission.enabled = true;
                
                MySM._screenshakeActivator.ScreenShakeContinuousOn(MySM._screenshakeActivator.DiveData);
            }

            public override void Exit(MovementStateInput i)
            {
                MySM._screenshakeActivator.ScreenShakeContinuousOff(MySM._screenshakeActivator.DiveData);
                // var divePEmission = MySM._diveParticles.emission;
                // divePEmission.enabled = false;
                base.Exit(i);
                
                //MySM._drillEmitter.Stop();
            }

            public override void JumpPressed()
            {
                base.JumpPressed();
                if (Input.canDoubleJump)
                {
                    DoubleJump();
                    MySM.Transition<Airborne>();
                }
            }

            public override void SetGrounded(bool isGrounded, bool isMovingUp)
            {
                base.SetGrounded(isGrounded, isMovingUp);
                if (isGrounded)
                {
                    MySM.Transition<Dogoing>();
                }
            }

            public override void FixedUpdate() {
                smActor.UpdateWhileDiving();
            }

            public override Vector2 MoveX(PlayerActor p, Vector2 velocity, int direction)
            {
                UpdateSpriteFacing(direction);
                return p.CalcMovementX(direction, MyCore.MaxAirAcceleration, MyCore.AirResistance);
            }
        }
    }
}