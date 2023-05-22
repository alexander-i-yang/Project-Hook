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

            public override void MoveX(int moveDirection)
            {
                UpdateSpriteFacing(moveDirection);
                int vxSign = (int) Mathf.Sign(smActor.velocityX);
                int acceleration = moveDirection == vxSign || moveDirection == 0 ? 100 : core.MaxDeceleration;
                smActor.UpdateMovementX(moveDirection, acceleration);
            }
        }
    }
}