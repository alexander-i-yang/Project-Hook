using A2DK.Phys;
using ASK.Core;
using Helpers;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerCore))]
    public class PlayerJostleBehavior : JostleBehavior
    {
        private Vector2 _gracePrevV;
        private GameTimer2 _graceTimer;
        [SerializeField] private float GraceTime;
        
        protected override bool FloorStopped()
        {
            if (prevRidingOn == jostledActor.GetBelowPhysObj()) return false;
            return base.FloorStopped();
        }

        protected override Vector2 ResolveApplyV()
        {
            Vector2 ret = base.ResolveApplyV();
            if (_graceTimer != null && GameTimer2.TimerRunning(_graceTimer))
            {
                GameTimerManager.Instance.RemoveTimer(_graceTimer);
                ret = _gracePrevV;
            };
            return ret;
        }
        
        public override Vector2 ResolveRidingOn()
        {
            if (base.FloorStopped())
            {
                _gracePrevV = prevRidingV;
                _graceTimer = GameTimerManager.Instance.StartTimer(GraceTime, () => { }, IncrementType.FIXED_UPDATE);
            }
            return base.ResolveRidingOn();
        }
    }
}