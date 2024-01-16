using ASK.Core;
using ASK.Helpers;
using Combat;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public partial class ParryStateMachine : PlayerStateMachine<ParryStateMachine, ParryStateMachine.ParryState, ParryStateInput> {
        //Expose to inspector
        public UnityEvent<ParryStateMachine> OnAbilityStateChange;
        [SerializeField] public float BulletTimeScale;

        #region Overrides
        protected override void SetInitialState() 
        {
            SetState<Idle>();
        }

        protected void OnEnable()
        {
            StateTransition += InvokeUnityStateChangeEvent;
        }

        protected void OnDisable()
        {
            StateTransition -= InvokeUnityStateChangeEvent;
        }

        private void InvokeUnityStateChangeEvent()
        {
            OnAbilityStateChange?.Invoke(this);
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            CurrState.ReadParryInput(MyCore.Input.GetParryInput());
        }
        #endregion

        public Vector2 ProcessCollideHorizontal(Vector2 oldV, Vector2 newV) => CurrState.ProcessCollideHorizontal(oldV, newV);

        public Vector2 GetAimInputPos() => MyCore.Input.GetAimPos(MyPhysObj.transform.position);
    }
}