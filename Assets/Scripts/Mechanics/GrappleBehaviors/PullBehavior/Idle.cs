using A2DK.Phys;
using UnityEngine;

namespace Mechanics.GrappleBehaviors.PullBehavior
{
    public class Idle : PullBehaviorState
    {
        public override void Enter(PullBehaviorStateInput i)
        {
            if (Input.KeepV)
            {
                MySM.MyPullBehavior.SetV(Input.BeforeStickyV);
            }

            Input.KeepV = false;
        }

        public override void AttachGrapple(GrapplerStateMachine grappler)
        {
            Input.Grappler = grappler;
            MySM.Transition<Pulling>();
        }

        public override void ContinuousGrapplePos(Vector2 grappleVector, Actor grappledActor) {}
    }
}