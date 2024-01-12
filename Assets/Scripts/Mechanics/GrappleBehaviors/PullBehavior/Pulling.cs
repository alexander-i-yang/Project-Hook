using A2DK.Phys;
using UnityEngine;

namespace Mechanics.GrappleBehaviors.PullBehavior
{
    public class Pulling : PullBehaviorState
    {
        public override void DetachGrapple()
        {
            MySM.Transition<Idle>();
        }

        public override void StickyEnter()
        {
            MySM.Transition<Sticky>();
        }

        public override void ContinuousGrapplePos(Vector2 grappleVector, Actor grappledActor, float distanceScale,
            float minPullV, float grappleLerp)
        {
            float newMag = grappleVector.magnitude * distanceScale;
            newMag = Mathf.Max(minPullV, newMag);

            Vector2 targetV = grappleVector.normalized * newMag;
            
            grappledActor.ApplyVelocity(grappleLerp * targetV);
            grappledActor.SetVelocity(Vector3.Project(grappledActor.velocity, grappleVector));
        }
    }
}