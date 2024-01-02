using A2DK.Phys;
using Helpers;
using UnityEngine;

namespace Player
{
    public class PlayerJostleBehavior : JostleBehavior
    {
        private Vector2 _gracePrevV;
        private GameTimer2 _graceTimer;
        [SerializeField] private float GraceTime;
        
        protected override bool FloorStopped()
        {
            if (prevRidingOn == physObj.CalcRiding()) return false;
            return base.FloorStopped();
        }

        protected override bool ShouldApplyV()
        {
            
            return base.ShouldApplyV();
        }

        protected override Vector2 ResolveApplyV()
        {
            if (_graceTimer != null && GameTimer2.TimerRunning(_graceTimer))
            {
                GameTimerManager.Instance.RemoveTimer(_graceTimer);
                return _gracePrevV;
            };
            return base.ResolveApplyV();
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