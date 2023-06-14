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
        }
    }
}