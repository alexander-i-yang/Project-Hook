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
                if (GameTimer.GetTimerState(Input.jumpBufferTimer) == TimerState.Running && !MySM.PrevStateEquals<Diving>())
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

            public override Vector2 MoveX(PlayerActor p, Vector2 velocity, int direction)
            {
                UpdateSpriteFacing(direction);
                AnimSetRunning(direction != 0);
                return p.CalcMovementX(direction, MyCore.MaxAcceleration, MyCore.MaxDeceleration);
            }

            public override void FixedUpdate() {
                base.FixedUpdate();
                if (!smActor.IsGrounded()) {
                    MySM.Transition<Airborne>();
                }
            }
        }
    }
}