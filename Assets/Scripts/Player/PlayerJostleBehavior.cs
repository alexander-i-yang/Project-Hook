using A2DK.Phys;
using Helpers;
using UnityEngine;

namespace Player
{
    public class PlayerJostleBehavior : JostleBehavior
    {
        [SerializeField] private float GraceTime;
        private Vector2 _gracePrevV;

        private GameTimer2 _graceTimer;
        
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

        
        //When ridingOn stops, start a timer.
        //If you jump before the timer ends, apply v.
        
        //when ridingOn.v == 0 && _prevRV != 0, start _graceTimer. _gracePrevV = _prevV.
        //Override JumpedOff. ||= _graceTimer notFinished.
        //When ShouldApplyV called, ret = _gracePrevV.
        
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