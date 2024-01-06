using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class MovementStateMachine
    {
        public class Airborne : MovementState
        {
            private GameTimer _jumpCoyoteTimer;

            public override void Enter(MovementStateInput i)
            {
                // PlayAnimation(PlayerAnimations.JUMP_INIT);
                if (!Input.jumpedFromGround)
                {
                    _jumpCoyoteTimer = GameTimer.StartNewTimer(MyCore.JumpCoyoteTime, "Jump Coyote Timer");
                }
            }

            public override void JumpPressed()
            {
                TimerState coyoteState = GameTimer.GetTimerState(_jumpCoyoteTimer);
                if (coyoteState == TimerState.Running)
                {
                    JumpFromGround();
                    base.JumpPressed();
                    return;
                } else if (Input.canDoubleJump)
                {
                    DoubleJump();
                    return;
                }
                
                else
                {
                    base.JumpPressed();
                }
            }

            public override void JumpReleased()
            {
                base.JumpReleased();
                TryJumpCut();
            }

            public override void DivePressed()
            {
                base.DivePressed();
                if (Input.canDive)
                {
                    MySM.Transition<Diving>();
                }
            }

            public override void SetGrounded(bool isGrounded, bool isMovingUp)
            {
                base.SetGrounded(isGrounded, isMovingUp);
                if (!isMovingUp && isGrounded) {
                    // PlayAnimation(PlayerAnimations.LANDING);
                    MySM.Transition<Grounded>();
                }
            }

            public override Vector2 MoveX(Vector2 velocity, int direction)
            {
                UpdateSpriteFacing(direction);
                return MySM.MyPhysObj.CalcMovementX(direction, MyCore.MaxAirAcceleration, MyCore.AirResistance);
            }

            public override void FixedUpdate()
            {
                MySM.MyPhysObj.Fall();
                GameTimer.FixedUpdate(_jumpCoyoteTimer);
            }
        }
    }
}