using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class PlayerStateMachine
    {
        public class ExtendGrapple : Airborne
        {
            private float _grappleDuration;
            
            public override void Enter(PlayerStateInput i) {
                _grappleDuration = 0;
                Input.curGrappleExtendPos = smActor.transform.position;
            }
            
            public override void FixedUpdate()
            {
                base.FixedUpdate();
                _grappleDuration += Game.TimeManager.FixedDeltaTime;
                var updateData = smActor.GrappleExtendUpdate(_grappleDuration, Input.currentGrapplePos);
                Input.curGrappleExtendPos = updateData.curPoint;
                if (updateData.hit) {
                    MySM.Transition<Grappling>();
                }
                // GameTimer.FixedUpdate(_extendTimer);
                // Input.curGrappleExtendPercent = 1-_extendTimer.TimerValue/core.GrappleExtendTime;
                // if (GameTimer.GetTimerState(_extendTimer) == TimerState.Finished) {
                //     MySM.Transition<Grappling>();
                // }
            }

            public override void GrappleStarted() {}

            public override void GrappleFinished() {
                MySM.Transition<Airborne>();
            }
        }
    }
}