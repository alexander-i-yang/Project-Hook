using ASK.Core;
using Phys.PhysObjStateMachine;
using UnityEngine;

namespace Mechanics
{
    public class ZiplineStateMachine : PhysObjStateMachine<ZiplineStateMachine, ZiplineState, ZiplineStateInput, Zipline>
    {
        protected override void SetInitialState()
        {
            SetState<ZiplineStateStart>();
        }

        public void TouchGrapple() => CurrState.TouchGrapple();

        public Vector2 CalculateVelocity() => CurrState.CalculateVelocity();
    }

    public abstract class ZiplineState : PhysObjStateMachine.PhysObjState<ZiplineStateMachine, ZiplineState, ZiplineStateInput, Zipline>
    {
        public abstract void TouchGrapple();

        public abstract Vector2 CalculateVelocity();
    }
    
    public class ZiplineStateInput : PhysObjStateInput
    {
        
    }
}