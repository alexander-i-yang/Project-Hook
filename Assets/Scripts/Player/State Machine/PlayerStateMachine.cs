using ASK.Core;
using ASK.Helpers;
using VFX;

using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public partial class PlayerStateMachine : StateMachine<PlayerStateMachine, PlayerStateMachine.PlayerState, PlayerStateInput> {
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
        public UnityEvent<PlayerStateMachine> OnPlayerStateChange;
        [SerializeField] private ParticleSystem _diveParticles;

        public bool UsingDrill => IsOnState<Diving>() || IsOnState<Dogoing>();
        public bool DrillingIntoGround => IsOnState<Dogoing>();

        private PlayerScreenShakeActivator _screenshakeActivator;

        private bool _hasInputted;

        #region Overrides
        protected override void SetInitialState() 
        {
            SetState<Grounded>();
            // _playerAnim.Play(PlayerAnimations.SLEEPING);
        }

        protected override void Init()
        {
            _core = GetComponent<PlayerCore>();
            //_deathAnim = GetComponentInChildren<DeathAnimationManager>();
            _spriteR = GetComponentInChildren<SpriteRenderer>();
            _screenshakeActivator = GetComponent<PlayerScreenShakeActivator>();
            
            //_drillEmitter = GetComponentInChildren<StudioEventEmitter>();
        }

        protected void OnEnable()
        {
            StateTransition += InvokeUnityStateChangeEvent;
            MyCore.SpawnManager.OnPlayerRespawn += OnRespawn;
        }

        protected void OnDisable()
        {
            StateTransition -= InvokeUnityStateChangeEvent;
            MyCore.SpawnManager.OnPlayerRespawn -= OnRespawn;
        }

        private void InvokeUnityStateChangeEvent()
        {
            OnPlayerStateChange?.Invoke(this);
        }

        protected override void Update()
        {
            base.Update();
            
            if (!_hasInputted && MyCore.Input.AnyKeyPressed()) _hasInputted = true;

            if (MyCore.Input.JumpStarted())
            {
                CurrState.JumpPressed();
            }

            if (MyCore.Input.JumpFinished())
            {
                CurrState.JumpReleased();
            }

            if (MyCore.Input.DiveStarted())
            {
                CurrState.DivePressed();
            }

            if (MyCore.Input.GrappleStarted())
            {
                CurrState.GrappleStarted();
            }

            if (MyCore.Input.GrappleFinished())
            {
                CurrState.GrappleFinished();
            }

            if (MyCore.Input.RetryStarted())
            {
                MyCore.Actor.Die(v => v);
            }
            
            CurrInput.moveDirection = MyCore.Input.GetMovementInput();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            GameTimer.FixedUpdate(CurrInput.jumpBufferTimer);
            CurrState.SetGrounded(MyCore.Actor.IsGrounded(), MyCore.Actor.IsMovingUp);
            CurrState.MoveX(CurrInput.moveDirection);
        }
        #endregion

        public void RefreshAbilities()
        {
            CurrState.RefreshAbilities();
        }

        public bool IsGrappling() {return IsOnState<Grappling>();}

        public void OnDeath()
        {
            // _spriteR.SetAlpha(0);
            // CurrInput.diePos = diePos;
            Transition<Dead>();
        }

        public void OnRespawn()
        {
            Transition<Airborne>();
        }

        public Vector2 GetGrapplePos() {
            return CurrInput.currentGrapplePos;
        }
    }
}