using ASK.Core;
using ASK.Helpers;
using VFX;

using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public partial class AbilityStateMachine : StateMachine<AbilityStateMachine, AbilityStateMachine.AbilityState, AbilityStateInput> {
        private PlayerCore _core;
        protected PlayerCore MyCore
        {
            get
            {
                if (_core == null) _core = GetComponent<PlayerCore>();
                return _core;
            }
        }

        private SpriteRenderer _spriteR;

        //Expose to inspector
        public UnityEvent<AbilityStateMachine> OnAbilityStateChange;

        private PlayerScreenShakeActivator _screenshakeActivator;

        private bool _hasInputted;

        #region Overrides
        protected override void SetInitialState() 
        {
            SetState<Idle>();
        }

        protected override void Init()
        {
            _core = GetComponent<PlayerCore>();
            _spriteR = GetComponentInChildren<SpriteRenderer>();
            _screenshakeActivator = GetComponent<PlayerScreenShakeActivator>();
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
            
            if (MyCore.Input.GrappleStarted())
            {
                CurrState.GrappleStarted();
            }

            if (MyCore.Input.GrappleFinished())
            {
                CurrState.GrappleFinished();
            }
        }

        public void CollideHorizontal() {
            CurrState.CollideHorizontal();
        }

        public void CollideVertical() {
            CurrState.CollideVertical();
        }

        #endregion

        public bool IsGrappling() => IsOnState<Grappling>();

        public bool IsGrappleExtending() => IsOnState<ExtendGrapple>();

        public Vector2 GetGrappleInputPos() => MyCore.Input.GetMousePos();

        public Vector2 GetGrapplePos() => CurrInput.currentGrapplePos;

        public Vector2 GetGrappleExtendPos() => CurrInput.curGrappleExtendPos;
    }
}