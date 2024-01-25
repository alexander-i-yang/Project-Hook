using ASK.Core;
using ASK.Helpers;

using UnityEditor;
using UnityEngine;

namespace Player
{
    public partial class MovementStateMachine
    {
        public abstract class MovementState : PlayerStateMachine.PlayerState<MovementStateMachine, MovementState, MovementStateInput>
        {
            public virtual void JumpPressed()
            {
                Input.jumpBufferTimer = GameTimer.StartNewTimer(MyCore.JumpBufferTime, "Jump Buffer Timer");
            }

            protected void PlayAnimation(PlayerAnimations p)
            {
                if (MySM._hasInputted) animManager.Play(p);
            }

            protected void AnimSetRunning(bool e)
            {
                if (MySM._hasInputted) animManager.Animator.SetBool("Running", e);
            }

            public virtual void JumpReleased() { }
            public virtual void DivePressed() { }
            public virtual void SetGrounded(bool isGrounded, bool isMovingUp) { }

            public void RefreshAbilities()
            {
                Input.canDoubleJump = true;
                Input.canDive = true;
            }

            protected void UpdateSpriteFacing(int moveDirection)
            {
                if (moveDirection != 0)
                {
                    MySM._spriteR.transform.localScale = new Vector3(moveDirection, 1, 0);
                }
            }

            protected void JumpFromGround()
            {
                Input.jumpedFromGround = true;
                Input.canJumpCut = true;
                GameTimer.Clear(Input.jumpBufferTimer);
                // PlayAnimation(PlayerAnimations.JUMP_INIT);
                MySM.MyPhysObj.JumpFromGround(MyCore.JumpHeight);
                SetGrounded(false, true); 
            }

            protected void DoubleJump()
            {
                Input.canJumpCut = true;
                MySM.MyPhysObj.DoubleJump(Input.moveDirection);
                Input.canDoubleJump = false;
                SetGrounded(false, true);
            }

            protected void TryJumpCut()
            {
                if (Input.canJumpCut)
                {
                    MySM.MyPhysObj.JumpCut();
                    Input.canJumpCut = false;
                }
            }

            public virtual Vector2 Fall(Vector2 v) => v;

            public abstract Vector2 PhysTick(Vector2 velocity, Vector2 newV, int direction);
        }
    }
}