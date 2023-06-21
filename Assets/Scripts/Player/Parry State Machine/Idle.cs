using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class ParryStateMachine
    {
        public class Idle : ParryState
        {
            public override void ParryStarted() {
                MySM.Transition<Parrying>();
                print("Parry started!");
            }

            public override Vector2 ProcessCollideHorizontal(Vector2 oldV, Vector2 newV)
            {
                MySM.Transition<HitWallBuffer>();
                return base.ProcessCollideHorizontal(oldV, newV);
            }
        }
    }
}