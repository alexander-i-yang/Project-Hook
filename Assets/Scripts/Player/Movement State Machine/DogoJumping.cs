using ASK.Helpers;
using System.Collections;
using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class MovementStateMachine
    {
        public class DogoJumping : MovementState
        {
            private GameTimer _dogoJumpTimer;

            public override void Enter(MovementStateInput i)
            {
                bool conserveMomentum = GameTimerWindowed.GetTimerState(i.ultraTimer) == TimerStateWindowed.InWindow;
                MySM.StartCoroutine(DogoJumpRoutine(conserveMomentum, i.oldVelocity));
                RefreshAbilities();
            }

            private int GetDogoJumpDirection() {
                int facing = MySM.MyPhysObj.Facing;
                int moveDir = Input.moveDirection;
                if (moveDir == 0) moveDir = facing;
                return moveDir;
            }

            private IEnumerator DogoJumpRoutine(bool conserveMomentum, double oldXV)
            {
                Input.canJumpCut = true;
                _dogoJumpTimer = GameTimer.StartNewTimer(MyCore.DogoJumpTime);
                int jumpDir = GetDogoJumpDirection();
                MySM.MyPhysObj.DogoJump(jumpDir, conserveMomentum, oldXV);
                int oldJumpDir = jumpDir;
                
                yield return Helper.DelayAction(MyCore.DogoJumpGraceTime, () => {
                    jumpDir = GetDogoJumpDirection();
                    if (jumpDir != oldJumpDir)
                    {
                        _dogoJumpTimer = GameTimer.StartNewTimer(MyCore.DogoJumpTime);
                        MySM.MyPhysObj.DogoJump(jumpDir, conserveMomentum, oldXV);
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

            public override Vector2 MoveX(Vector2 velocity, int direction)
            {
                UpdateSpriteFacing(direction);
                return MySM.MyPhysObj.CalcMovementX(direction, MyCore.MaxAirAcceleration, MyCore.AirResistance);
            }

            public override void FixedUpdate()
            {
                GameTimer.FixedUpdate(_dogoJumpTimer);
                MySM.MyPhysObj.Fall();
                if (GameTimer.GetTimerState(_dogoJumpTimer) == TimerState.Finished)
                {
                    MySM.Transition<Airborne>();
                }
            }
        }
    }
}