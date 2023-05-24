using ASK.Core;
using UnityEngine;

namespace Player
{
    public partial class PlayerStateMachine
    {
        public class AfterGrapple : Airborne
        {
            public override void FixedUpdate()
            {
                smActor.Fall();
            }
        }
    }
}