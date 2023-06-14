using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class ParryStateMachine
    {
        public class Parrying : ParryState
        {
            public override void ParryStarted() {
                // MySM.Transition<Parrying>();
            }

            public override void OnCollide() {

            }

            public override Vector2 ProcessCollideHorizontal(Vector2 oldV, Vector2 newV) {
                MySM.Transition<Idle>();
                print("Parry!");
                return Vector2.right * -2 * oldV.x;
            }
        }
    }
}