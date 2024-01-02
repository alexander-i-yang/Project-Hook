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
        private PhysObj _physObj;

        private PhysObj _ridingOn;
        private Vector2 _prevRidingV; //Prev velocity of RidingOn.
        private PhysObj _prevRidingOn;

        void Awake()
        {
            _physObj = GetComponent<PhysObj>();
        }
        
        public bool IsRiding(PhysObj p) {
            return _ridingOn == p;
        }
        
        protected virtual bool ShouldApplyV()
        {
            bool jumpedOff = _prevRidingOn != null && _ridingOn == null;
            bool floorStopped = _prevRidingV != Vector2.zero && _ridingOn != null && _ridingOn.velocity == Vector2.zero;
            return jumpedOff || floorStopped;
        }
        
        /**
         * Set _ridingOn to whatever CalcRiding returns.
         */
        public Vector2 ResolveRidingOn()
        {
            Vector2 ret = Vector2.zero;
            _ridingOn = _physObj.CalcRiding();
            if (ShouldApplyV())
            {
                ret = _prevRidingV;
            }
            _prevRidingOn = _ridingOn;
            _prevRidingV = _ridingOn == null ? Vector2.zero : _ridingOn.velocity;
            return ret;
        }
    }
}