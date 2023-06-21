using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class ParryStateMachine
    {
        public class Parrying : ParryState
        {
            private GameTimer _parryTimer;

            public override void Enter(ParryStateInput i)
            {
                base.Enter(i);
                _parryTimer = GameTimer.StartNewTimer(MyCore.ParryPreCollisionWindow);
            }

            public override void ParryStarted() {
                // MySM.Transition<Parrying>();
            }

            public override void OnCollide() {

            }

            public override void FixedUpdate()
            {
                GameTimer.FixedUpdate(_parryTimer);
                if (GameTimer.GetTimerState(_parryTimer) == TimerState.Finished) {
                    MySM.Transition<Idle>();
                }
            }

            public override Vector2 ProcessCollideHorizontal(Vector2 oldV, Vector2 newV) {
                MySM.Transition<Idle>();
                return Vector2.left * 2 * oldV.x * MyCore.ParryVMult;
            }
        }
    }
}