using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class PlayerStateMachine
    {
        public class ExtendGrapple : Airborne
        {
            private GameTimer _extendTimer;
            
            public override void Enter(PlayerStateInput i) {
                _extendTimer = GameTimer.StartNewTimer(core.GrappleExtendTime);
            }
            
            public override void FixedUpdate()
            {
                GameTimer.FixedUpdate(_extendTimer);
                if (GameTimer.GetTimerState(_extendTimer) == TimerState.Finished) {
                    MySM.Transition<Grappling>();
                }
            }

            public override void GrappleStarted() {}

            public override void GrappleFinished() {
                MySM.Transition<Airborne>();
            }

            public override void MoveX(int moveDirection)
            {
                UpdateSpriteFacing(moveDirection);
                int vxSign = (int) Mathf.Sign(smActor.velocityX);
                int acceleration = moveDirection == vxSign || moveDirection == 0 ? 100 : core.MaxDeceleration;
                smActor.UpdateMovementX(moveDirection, acceleration);
            }
        }
    }
}