using ASK.Core;
using UnityEngine;

namespace Player
{
    public class Parrying : ParryStateMachine.ParryState
    {
        private GameTimer _parryTimer;

        public override void Enter(ParryStateInput i)
        {
            MySM.MyCore.Parrier.Parry(i.CurAimPos, MySM.MyPhysObj.velocity);
            _parryTimer = GameTimer.StartNewTimer(MyCore.ParryPreCollisionWindow);
        }

        public override void Exit(ParryStateInput i)
        {
            MySM.MyCore.Parrier.Idle();
        }

        public override void FixedUpdate()
        {
            GameTimer.FixedUpdate(_parryTimer);
            if (GameTimer.GetTimerState(_parryTimer) == TimerState.Finished)
            {
                MySM.Transition<ParryStateMachine.Idle>();
            }
        }

        public override Vector2 ProcessCollideHorizontal(Vector2 oldV, Vector2 newV)
        {
            MySM.Transition<ParryStateMachine.Idle>();
            return Vector2.left * 2 * oldV.x * MyCore.ParryVMult;
        }
    }
}