using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class MovementStateMachine {
        public class Grounded : MovementState
        {
            public override void Enter(MovementStateInput i)
            {
                //PlayerAnim.ChangeState(PlayerAnimations.IDLE);
                Input.jumpedFromGround = false;
                RefreshAbilities();
                // smActor.Land();
                if (GameTimer.GetTimerState(Input.jumpBufferTimer) == TimerState.Running)
                {
                    JumpFromGround();
                }
            }

            public override void JumpPressed()
            {
                // base.JumpPressed();
                JumpFromGround();
            }

            public override void SetGrounded(bool isGrounded, bool isMovingUp)
            {
                base.SetGrounded(isGrounded, isMovingUp);
                if (!isGrounded)
                {
                    MySM.Transition<Airborne>();
                }
            }

            public override Vector2 PhysTick(Vector2 velocity, Vector2 newV, int direction)
            {
                AnimSetRunning(direction != 0);
                return MySM.MyPhysObj.CalcMovementX(velocity, direction, MyCore.MaxAcceleration, MyCore.MaxDeceleration);
            }

            public override void FixedUpdate() {
                base.FixedUpdate();
                if (!MySM.MyPhysObj.IsGrounded()) {
                    MySM.Transition<Airborne>();
                }
            }
        }
    }
}