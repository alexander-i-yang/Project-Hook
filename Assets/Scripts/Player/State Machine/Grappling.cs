using ASK.Core;

namespace Player
{
    public partial class PlayerStateMachine
    {
        public class Grappling : PlayerState
        {
            private GameTimer _grappleTimer;

            public override void Enter(PlayerStateInput i)
            {
                print("Enter grapple");
                _grappleTimer = GameTimer.StartNewTimer(core.GrappleWarmTime, "Jump Coyote Timer");
            }

            public override void SetGrounded(bool isGrounded, bool isMovingUp)
            {
                /*base.SetGrounded(isGrounded, isMovingUp);
                if (!isMovingUp && isGrounded) {
                    // PlayAnimation(PlayerAnimations.LANDING);
                    MySM.Transition<Grounded>();
                }*/
            }

            /*public override void MoveX(int moveDirection)
            {
                UpdateSpriteFacing(moveDirection);
                smActor.UpdateMovementX(moveDirection, core.MaxAirAcceleration);
            }*/

            public override void FixedUpdate()
            {
                smActor.GrappleUpdate(Input.currentGrapplePos, _grappleTimer.TimerValue/core.GrappleWarmTime);
                GameTimer.FixedUpdate(_grappleTimer);
            }

            public override void GrappleFinished()
            {
                base.GrappleFinished();
                MySM.Transition<Airborne>();
            }
        }
    }
}