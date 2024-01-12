using A2DK.Phys;
using Mechanics.GrappleBehaviors.PullBehavior;
using Phys.PhysObjStateMachine;
using UnityEngine;

namespace Mechanics
{
    public class PullBehaviorStateMachine : PhysObjStateMachine<PullBehaviorStateMachine, PullBehaviorState, PullBehaviorStateInput, Actor>
    {
        protected override void SetInitialState()
        {
            SetState<Idle>();
        }
    }

    public abstract class PullBehaviorState : PhysObjStateMachine.PhysObjState<PullBehaviorStateMachine, PullBehaviorState, PullBehaviorStateInput, Actor>
    {
        public virtual void AttachGrapple() {}
        public virtual void DetachGrapple() {}
        public virtual void StickyEnter() {}
        public virtual void StickyExit() {}

        public abstract void ContinuousGrapplePos(Vector2 grappleVector, Actor grappledActor, float distanceScale,
            float minPullV, float grappleLerp);
    }

    public class PullBehaviorStateInput : PhysObjStateInput
    {
        
    }
}