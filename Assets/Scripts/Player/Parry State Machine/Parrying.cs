using ASK.Core;
using UnityEngine;

namespace Player
{
    public class Parrying : ParryStateMachine.ParryState
    {
        private GameTimer _parryTimer;

        public override void Enter(ParryStateInput i)
        {
            MySM.MyCore.Puncher.Punch(i.CurAimPos, MySM.MyPhysObj.velocity, MySM.MyPhysObj.ParryBounce);
            _parryTimer = GameTimer.StartNewTimer(MyCore.ParryPreCollisionWindow);
        }

        public override void Exit(ParryStateInput i)
        {
            MySM.MyCore.Puncher.Idle();
        }

        public override void FixedUpdate()
        {
            GameTimer.FixedUpdate(_parryTimer);
            if (GameTimer.GetTimerState(_parryTimer) == TimerState.Finished)
            {
                MySM.Transition<ParryStateMachine.Idle>();
            }
        }
    }
}