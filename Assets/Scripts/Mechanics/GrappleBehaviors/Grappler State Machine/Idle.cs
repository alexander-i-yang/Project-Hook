using ASK.Core;
using UnityEngine;

namespace Mechanics
{
    public partial class GrapplerStateMachine
    {
        public class Idle : GrappleState
        {
            private float _grappleDuration;
            
            public override void Enter(GrappleStateInput i) {
                
            }

            public override void GrappleStarted() {
                MySM.Transition<ExtendGrapple>();
                return;
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