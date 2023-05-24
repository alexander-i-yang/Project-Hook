using ASK.Core;

namespace Player
{
    public partial class PlayerStateMachine
    {
        public class Grappling : PlayerState
        {

            public override void Enter(PlayerStateInput i)
            {
                // _grappleTimer = GameTimer.StartNewTimer(core.GrappleWarmTime);
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
                smActor.GrappleUpdate(Input.currentGrapplePos, 0);
                // GameTimer.FixedUpdate(_grappleTimer);
            }

            public override void GrappleFinished()
            {
                base.GrappleFinished();
                MySM.Transition<AfterGrapple>();
            }
        }
    }
}