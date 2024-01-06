using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace Mechanics
{
    public class Airborne : CrateState
    {
        public override void SetGrounded(bool isGrounded, bool isMovingUp)
        {
            base.SetGrounded(isGrounded, isMovingUp);
            if (!isMovingUp && isGrounded) {
                // PlayAnimation(PlayerAnimations.LANDING);
                MySM.Transition<Grounded>();
            }
        }
        
        public override float ApplyXFriction(float prevXVelocity)
        {
            var crate = MySM.MyPhysObj;
            return crate.ApplyXFriction(prevXVelocity, crate.AirborneFrictionAccel);
        }

        public override void FixedUpdate()
        {
            MySM.MyPhysObj.Fall();
        }
    }
}