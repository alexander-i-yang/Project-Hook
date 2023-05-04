using ASK.Core;
using ASK.Helpers;

using UnityEngine;

namespace Player
{
    public partial class PlayerStateMachine
    {
        public abstract class PlayerState : State<PlayerStateMachine, PlayerState, PlayerStateInput>
        {
            protected PlayerCore core => MySM.MyCore;
            protected PlayerSpawnManager spawnManager => core.SpawnManager;
            protected PlayerAnimationStateManager animManager => core.AnimManager;
            protected PlayerActor smActor => core.Actor;
            
            public virtual void JumpPressed()
            {
                Input.jumpBufferTimer = GameTimer.StartNewTimer(core.JumpBufferTime, "Jump Buffer Timer");
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
            public virtual void MoveX(int moveDirection) { }

            public void RefreshAbilities()
            {
                Input.canDoubleJump = true;
                Input.canDive = true;
            }

            protected void UpdateSpriteFacing(int moveDirection)
            {
                if (moveDirection != 0)
                {
                    MySM._spriteR.flipX = moveDirection == -1;
                }
            }

            protected void JumpFromGround()
            {
                Input.jumpedFromGround = true;
                Input.canJumpCut = true;
                GameTimer.Clear(Input.jumpBufferTimer);
                // PlayAnimation(PlayerAnimations.JUMP_INIT);
                smActor.JumpFromGround(core.JumpHeight);
                SetGrounded(false, true); 
            }

            protected void DoubleJump()
            {
                Input.canJumpCut = true;
                smActor.DoubleJump(core.DoubleJumpHeight, Input.moveDirection);
                Input.canDoubleJump = false;
                SetGrounded(false, true);
            }

            protected void TryJumpCut()
            {
                if (Input.canJumpCut)
                {
                    smActor.JumpCut();
                    Input.canJumpCut = false;
                }
            }
        }
    }
}