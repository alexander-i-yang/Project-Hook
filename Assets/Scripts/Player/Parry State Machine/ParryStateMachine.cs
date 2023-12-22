using ASK.Core;
using ASK.Helpers;

using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public partial class ParryStateMachine : PlayerStateMachine<ParryStateMachine, ParryStateMachine.ParryState, ParryStateInput> {
        private SpriteRenderer _spriteR;

        //Expose to inspector
        public UnityEvent<ParryStateMachine> OnAbilityStateChange;

        #region Overrides
        protected override void SetInitialState() 
        {
            SetState<Idle>();
        }

        protected override void Init()
        {
            base.Init();
            _spriteR = GetComponentInChildren<SpriteRenderer>();
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
        }

        protected override void Update()
        {
            base.Update();
            
            if (MyCore.Input.ParryStarted())
            {
                CurrState.ParryStarted();
            }
        }
        #endregion

        public Vector2 ProcessCollideHorizontal(Vector2 oldV, Vector2 newV) => CurrState.ProcessCollideHorizontal(oldV, newV);
    }
}