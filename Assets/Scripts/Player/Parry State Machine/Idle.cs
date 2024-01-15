using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class ParryStateMachine
    {
        public class Idle : ParryState
        {
            public override void ReadParryInput(bool parryInput)
            {
                if (parryInput) MySM.Transition<ParryAiming>();
            }
        }
    }
}