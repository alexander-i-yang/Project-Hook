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

        public override void StickyEnter(Vector2 myActorVelocity, Transform sticky)
        {
            Input.BeforeStickyV = myActorVelocity;
            Input.Sticky = sticky;
            MySM.Transition<Sticky>();
        }

        public override void ContinuousGrapplePos(Vector2 grappleVector, Actor grappledActor)
        {
            float newMag = grappleVector.magnitude * MySM.MyPullBehavior.DistanceScale;
            newMag = Mathf.Max(MySM.MyPullBehavior.MinPullV, newMag);

            Vector2 targetV = grappleVector.normalized * newMag;
            
            grappledActor.ApplyVelocity(MySM.MyPullBehavior.GrappleLerp * targetV);
            grappledActor.SetVelocity(Vector3.Project(grappledActor.velocity, grappleVector));
        }
    }
}