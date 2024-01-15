using A2DK.Phys;
using ASK.Core;
using Helpers;
using UnityEngine;

namespace Mechanics.GrappleBehaviors.PullBehavior
{
    public class Sticky : PullBehaviorState
    {
        private GameTimer2 _keepVGraceTimer;

        public override void Enter(PullBehaviorStateInput i)
        {
            _keepVGraceTimer = GameTimerManager.Instance.StartTimer(MySM.MyPullBehavior.KeepVGraceTime, () => {}, IncrementType.FIXED_UPDATE);
        }

        public override void Exit(PullBehaviorStateInput i)
        {
            if (GameTimer2.TimerRunning(_keepVGraceTimer))
            {
                Input.KeepV = true;
            }
        }

        public override void DetachGrapple()
        {
            MySM.Transition<Idle>();
        }

        public override void ContinuousGrapplePos(Vector2 grappleVector, Actor grappledActor)
        {
            if (grappledActor.velocity.magnitude > 0.01f)
            {
                _keepVGraceTimer = GameTimerManager.Instance.StartTimer(MySM.MyPullBehavior.KeepVGraceTime, () => {}, IncrementType.FIXED_UPDATE);
                Input.BeforeStickyV = grappledActor.velocity;
            }
            grappledActor.StickyPullMove(Input.Sticky.position - grappledActor.transform.position);
        }
    }
}