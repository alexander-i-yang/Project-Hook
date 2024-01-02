using System;
using UnityEngine;

namespace A2DK.Phys
{
    /**
     * Class receives input about the attached PhysObj and what PhysObj it's riding on/being pushed by.
     * Outputs new velocities that should be applied to the attached PhysObj.
     */
    [RequireComponent(typeof(PhysObj))]
    public class JostleBehavior : MonoBehaviour
    {
        protected PhysObj physObj;

        protected PhysObj ridingOn { get; private set; }
        //Prev velocity of RidingOn
        protected Vector2 prevRidingV { get; private set; }
        protected PhysObj prevRidingOn { get; set; }

        void Awake()
        {
            physObj = GetComponent<PhysObj>();
        }
        
        public bool IsRiding(PhysObj p) {
            return ridingOn == p;
        }
        
        /**
         * When there was a floor but now there's not
         */
        protected virtual bool JumpedOff() => prevRidingOn != null && ridingOn == null;
        
        /**
         * When the floor was moving but now it's not
         */
        protected virtual bool FloorStopped() => prevRidingV != Vector2.zero && ridingOn != null && ridingOn.velocity == Vector2.zero;
        
        protected virtual bool ShouldApplyV() => JumpedOff() || FloorStopped();
        
        /**
         * Set _ridingOn to whatever CalcRiding returns.
         * Should get called every frame.
         */
        public virtual Vector2 ResolveRidingOn()
        {
            Vector2 ret = Vector2.zero;
            ridingOn = physObj.CalcRiding();
            if (ShouldApplyV())
            {
                ret = ResolveApplyV();
            }
            prevRidingOn = ridingOn;
            prevRidingV = ridingOn == null ? Vector2.zero : ridingOn.velocity;
            return ret;
        }

        /**
         * Input previousApplyVelocity, output new apply velocity.
         * Only called when shouldApplyV.
         */
        protected virtual Vector2 ResolveApplyV()
        {
            return prevRidingV;
        }
    }
}