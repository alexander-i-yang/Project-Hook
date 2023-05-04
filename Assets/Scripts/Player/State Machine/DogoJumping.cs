using ASK.Helpers;
using System.Collections;
using ASK.Core;
using Mechanics;

namespace Player
{
    public partial class PlayerStateMachine
    {
        public class DogoJumping : PlayerState
        {
            private GameTimer _dogoJumpTimer;

            public override void Enter(PlayerStateInput i)
            {
                bool conserveMomentum = GameTimerWindowed.GetTimerState(i.ultraTimer) == TimerStateWindowed.InWindow;
                MySM.StartCoroutine(DogoJumpRoutine(conserveMomentum, i.oldVelocity));
                RefreshAbilities();
            }

            private int GetDogoJumpDirection() {
                int facing = smActor.Facing;
                int moveDir = Input.moveDirection;
                if (moveDir == 0) moveDir = facing;
                return moveDir;
            }

            private IEnumerator DogoJumpRoutine(bool conserveMomentum, double oldXV)
            {
                Input.canJumpCut = true;
                _dogoJumpTimer = GameTimer.StartNewTimer(core.DogoJumpTime);
                int jumpDir = GetDogoJumpDirection();
                smActor.DogoJump(jumpDir, conserveMomentum, oldXV);
                int oldJumpDir = jumpDir;
                
                yield return Helper.DelayAction(core.DogoJumpGraceTime, () => {
                    jumpDir = GetDogoJumpDirection();
                    if (jumpDir != oldJumpDir)
                    {
                        _dogoJumpTimer = GameTimer.StartNewTimer(core.DogoJumpTime);
                        smActor.DogoJump(jumpDir, conserveMomentum, oldXV);
                    }
                });
            }

            public override void JumpPressed()
            {
                if (Input.canDoubleJump)
                {
                    DoubleJump();
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
                if (isGrounded)
                {
                    MySM.Transition<Grounded>();
                }
            }

            public override void MoveX(int moveDirection)
            {
                UpdateSpriteFacing(moveDirection);
                smActor.UpdateMovementX(moveDirection, core.DogoJumpAcceleration);
            }

            public override void FixedUpdate()
            {
                GameTimer.FixedUpdate(_dogoJumpTimer);
                smActor.Fall();
                if (GameTimer.GetTimerState(_dogoJumpTimer) == TimerState.Finished)
                {
                    MySM.Transition<Airborne>();
                }
            }
        }
    }
}