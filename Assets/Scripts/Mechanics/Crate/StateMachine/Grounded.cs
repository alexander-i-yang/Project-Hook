using UnityEngine;

namespace Mechanics
{
    public class Grounded : CrateState
    {
        public override void FixedUpdate() {
            if (!MySM.MyPhysObj.IsGrounded()) {
                MySM.Transition<Airborne>();
            }
        }
        
        public override void SetGrounded(bool isGrounded, bool isMovingUp)
        {
            base.SetGrounded(isGrounded, isMovingUp);
            if (!isGrounded)
            {
                MySM.Transition<Airborne>();
            }
        }
        
        public override float ApplyXFriction(float prevXVelocity)
        {
            var crate = MySM.MyPhysObj;
            return crate.ApplyXFriction(prevXVelocity, crate.GroundedFrictionAccel);
        }
    }
}