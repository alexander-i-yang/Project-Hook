using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class MovementStateMachine
    {
        public class Dogoing : MovementState
        {
            public override void Enter(MovementStateInput i)
            {
                // PlayAnimation(PlayerAnimations.DOGOING);
                //MySM._drillEmitter.SetParameter("PlayerGrounded", 1);
                //MySM._drillEmitter.Play();
                i.oldVelocity = MySM.MyPhysObj.Dogo();
                i.ultraTimer = GameTimerWindowed.StartNewWindowedTimer(
                    MyCore.UltraTimeDelay, 
                    MyCore.UltraTimeWindow
                );
            }

            public override void JumpPressed()
            {
                base.JumpPressed();
                MySM.Transition<DogoJumping>();
            }

            public override void FixedUpdate()
            {
                GameTimer.FixedUpdate(Input.ultraTimer);

                base.FixedUpdate();
            }

            public override Vector2 MoveX(Vector2 velocity, int direction)
            {
                UpdateSpriteFacing(direction);
                return MySM.MyPhysObj.CalcMovementX(direction, MyCore.MaxAirAcceleration, MyCore.AirResistance);
            }

            public override void Update()
            {
                base.Update();
                if (MyCore.UltraHelper)
                {
                    if (GameTimerWindowed.GetTimerState(Input.ultraTimer) == TimerStateWindowed.InWindow)
                    {
                        MySM._spriteR.color = Color.green;
                    }
                    else
                    {
                        MySM._spriteR.color = Color.white;
                    }
                }
            }
            
            public override void Exit(MovementStateInput playerStateInput)
            {
                base.Exit(playerStateInput);
                //MySM._drillEmitter.Stop();
                if (MyCore.UltraHelper)
                {
                    MySM._spriteR.color = Color.white;
                }
            }

            public override void SetGrounded(bool isGrounded, bool isMovingUp)
            {
                base.SetGrounded(isGrounded, isMovingUp);
                // if (!isGrounded) {MySM.Transition<Diving>();}
            }
        }
    }
}