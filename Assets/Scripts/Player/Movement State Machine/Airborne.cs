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

            public override void SetGrounded(bool isGrounded, bool isMovingUp)
            {
                base.SetGrounded(isGrounded, isMovingUp);
                if (!isMovingUp && isGrounded) {
                    MySM.Transition<Grounded>();
                }
            }

            public override void FixedUpdate()
            {
                GameTimer.FixedUpdate(_jumpCoyoteTimer);
            }

            public override Vector2 Fall(Vector2 v) => MySM.MyPhysObj.CalcFall(v);

            public override Vector2 PhysTick(Vector2 velocity, Vector2 newV, int direction)
            {
                newV = MySM.MyPhysObj.CalcFall(newV);
                UpdateSpriteFacing(direction);
                return MySM.MyPhysObj.CalcMovementX(newV, direction, MyCore.MaxAirAcceleration, MyCore.AirResistance);
            }
        }
    }
}