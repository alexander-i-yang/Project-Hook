using ASK.Core;
using UnityEngine;

namespace Mechanics
{
    public partial class GrapplerStateMachine
    {
        public class Idle : GrappleState
        {
            private float _grappleDuration;

            public override void GrappleStarted() {
                MySM.Transition<ExtendGrapple>();
            }

            public override Vector2 PhysTick(Vector2 velocity, Vector2 newV, int getMovementInput) => newV;
        }
    }
}