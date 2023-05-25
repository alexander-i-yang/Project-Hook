using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class AbilityStateMachine
    {
        public class ExtendGrapple : AbilityState
        {
            private float _grappleDuration;
            
            public override void Enter(AbilityStateInput i) {
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
            }

            public override void GrappleStarted() {}

            public override void GrappleFinished() {
                MySM.Transition<Idle>();
            }
        }
    }
}