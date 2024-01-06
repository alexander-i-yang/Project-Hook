using Phys.PhysObjStateMachine;
using UnityEngine;

namespace Mechanics
{
    public class CrateStateMachine : PhysObjStateMachine<CrateStateMachine, CrateState, CrateStateInput, Crate>
    {
        protected override void SetInitialState() => SetState<Airborne>();

        public float ApplyXFriction(float velocityX) => CurrState.ApplyXFriction(velocityX);
    }

    public class CrateState : PhysObjStateMachine.PhysObjState<CrateStateMachine, CrateState, CrateStateInput, Crate>
    {
        public virtual void SetGrounded(bool isGrounded, bool isMovingUp) { }

        public virtual float ApplyXFriction(float prevXVelocity) => prevXVelocity;
    }

    public class CrateStateInput : PhysObjStateInput
    {
        
    }
}