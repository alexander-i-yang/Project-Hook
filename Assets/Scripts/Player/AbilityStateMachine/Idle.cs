using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class AbilityStateMachine
    {
        public class Idle : AbilityState
        {
            private float _grappleDuration;
            
            public override void Enter(AbilityStateInput i) {
                
            }

            public override void GrappleStarted() {
                Vector2 mousePos = MySM.GetGrappleInputPos();
                // Vector2 mousePos = (Vector2)smActor.transform.position + new Vector2(smActor.Facing * 2, 1) * 10;
                // Vector2? grapplePoint = smActor.GetGrapplePoint(mousePos);
                Vector2? grapplePoint = mousePos;
                if (grapplePoint != null)
                {
                    // Input.currentGrapplePos = grapplePoint.Value;
                    MySM.Transition<ExtendGrapple>();
                }
            }
        }
    }
}