using ASK.Core;
using UnityEngine;

namespace Mechanics
{
    public class ZiplineStateMachine : StateMachine<ZiplineStateMachine, ZiplineState, ZiplineStateInput>
    {
        protected override void SetInitialState()
        {
            SetState<ZiplineStateStart>();
        }

        public Zipline MyZ { get; private set; }

        public void TouchGrapple() => CurrState.TouchGrapple();

        protected override void Init()
        {
            MyZ = GetComponent<Zipline>();
        }

        public Vector2 CalculateVelocity() => CurrState.CalculateVelocity();
    }

    public abstract class ZiplineState : State<ZiplineStateMachine, ZiplineState, ZiplineStateInput>
    {
        public abstract void TouchGrapple();

        public abstract Vector2 CalculateVelocity();
    }
    
    public class ZiplineStateInput : StateInput
    {
        
    }
}