using A2DK.Phys;
using UnityEngine;

namespace Mechanics.GrappleBehaviors.PullBehavior
{
    public class Idle : PullBehaviorState
    {
        public override void AttachGrapple()
        {
            MySM.Transition<Pulling>();
        }

        public override void ContinuousGrapplePos(Vector2 grappleVector, Actor grappledActor, float distanceScale,
            float minPullV, float grappleLerp) {}
    }
}