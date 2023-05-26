using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class AbilityStateMachine
    {
        public class ExtendGrapple : AbilityState
        {
            private float _grappleDuration;
            private float _prevTimeScale;
            
            public override void Enter(AbilityStateInput i) {
                _grappleDuration = 0;
                smActor.ResetMyGrappleHook();
                _prevTimeScale = Game.TimeManager.TimeScale;
                Game.TimeManager.TimeScale = core.GrappleBulletTimeScale;
                Input.curGrappleExtendPos = smActor.transform.position;
            }

            public override void Exit(AbilityStateInput i) {
                base.Exit(i);
                Game.TimeManager.TimeScale = _prevTimeScale;
                Input.currentGrapplePos = Input.curGrappleExtendPos;
            }
            
            public override void FixedUpdate()
            {
                base.FixedUpdate();
                _grappleDuration += Game.TimeManager.FixedDeltaTime;
                var updateData = smActor.GrappleExtendUpdate(_grappleDuration, MySM.GetGrappleInputPos());
                Input.curGrappleExtendPos = updateData.curPoint;
                if (updateData.hit) {
                    MySM.Transition<Grappling>();
                }
            }

            public override void GrappleStarted() {}

            public override void GrappleFinished() {
                MySM.Transition<Idle>();
            }
        }
    }
}