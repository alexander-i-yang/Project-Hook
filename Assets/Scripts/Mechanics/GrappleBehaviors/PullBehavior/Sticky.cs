using A2DK.Phys;
using UnityEngine;

namespace Mechanics.GrappleBehaviors.PullBehavior
{
    public class Sticky : PullBehaviorState
    {
        public override void DetachGrapple()
        {
            MySM.Transition<Idle>();
        }

        public override void StickyExit()
        {
            MySM.Transition<Idle>();
        }

        public override void ContinuousGrapplePos(Vector2 grappleVector, Actor grappledActor, float distanceScale,
            float minPullV, float grappleLerp)
        {
            grappledActor.StickyPullMove(grappleVector);
        }
    }
}