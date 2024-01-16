using System;
using System.Data;
using Combat;
using UnityEngine;

using MyBox;

using UnityEngine.Serialization;

namespace Player
{
    //L: The purpose of this class is to ensure that all player components are initialized properly, and it helps keep all of the player properties in one place.
    [RequireComponent(typeof(PlayerActor))]
    [RequireComponent(typeof(MovementStateMachine))]
    [RequireComponent(typeof(PlayerGrapplerStateMachine))]
    [RequireComponent(typeof(ParryStateMachine))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerDeathManager))]
    public class PlayerCore : MonoBehaviour
    {
        public GrappleHook MyGrappleHook;

        #region Player Properties
        [Foldout("Move", true)]
        [SerializeField] public int MoveSpeed;

        [SerializeField] public int MaxAcceleration;

        [SerializeField] public int MaxAirAcceleration;

        [SerializeField] public int MaxDeceleration;

        [SerializeField] public int AirResistance;

        [Tooltip("Timer between the player crashing into a wall and them getting their speed back (called a cornerboost)")]
        [SerializeField] public float CornerboostTimer;

        [Tooltip("Cornerboost speed multiplier")]
        [SerializeField] public float CornerboostMultiplier;
        
        [Tooltip("Delay for jumping off whatever the player is riding")]
        [SerializeField]
        public float JostleBoostGraceTime;

        [Foldout("Jump", true)]
        [SerializeField] public int JumpHeight;

        [SerializeField] public int DoubleJumpHeight;
        
        [SerializeField] public int AddDoubleJumpHeight;

        [SerializeField] public float JumpCoyoteTime;

        [SerializeField] public float JumpBufferTime;

        [Tooltip("Y velocity after the player hits their head on the ceiling")]
        [SerializeField] public float BonkHeadV;

        [SerializeField, Range(0f, 1f)] public float JumpCutMultiplier;

        [Foldout("Dive", true)]
        [SerializeField] public int DiveVelocity;

        [SerializeField] public int DiveDeceleration;

        [Foldout("Dogo", true)]

        [Tooltip("Velocity multiplier from the discarded direction after you it a wall")]
        [SerializeField] public float HitWallGrappleMult;

        [Tooltip("Velocity multiplier for when you move left/right while grappling")]
        [SerializeField] public float MoveXGrappleMult;

        [Foldout("Parry", true)]
        [Tooltip("Velocity mult after parrying a wall")]
        [SerializeField] public float ParryVMult;

        [Tooltip("Time window before hitting a wall")]
        [SerializeField] public float ParryPreCollisionWindow;

        [Tooltip("Time window after hitting a wall")]
        [SerializeField] public float ParryPostCollisionWindow;
        
        [Tooltip("Velocity to add after punch bouncing")]
        [SerializeField] public float PunchBounceBoost;

        
        [Foldout("RoomTransitions", true)]
        [SerializeField, Range(0f, 1f)] public float RoomTransitionVCutX = 0.5f;

        [SerializeField, Range(0f, 1f)] public float RoomTransitionVCutY = 0.5f;
        
        [SerializeField] public float DeathTime;

        #endregion

        public MovementStateMachine MovementStateMachine { get; private set; }
        public PlayerGrapplerStateMachine GrapplerStateMachine { get; private set; }
        public ParryStateMachine ParryStateMachine { get; private set; }
        public PlayerInputController Input { get; private set; }
        public PlayerActor Actor { get; private set; }
        public Puncher Puncher { get; private set; }
        
        public PlayerDeathManager DeathManager { get; private set; }
        
        [NonSerialized] public PlayerAnimationStateManager AnimManager;
        
        private void Awake()
        {
            // InitializeSingleton(false); //L: Don't make player persistent, bc then there'll be multiple players OO
            MovementStateMachine = gameObject.GetComponent<MovementStateMachine>();
            GrapplerStateMachine = gameObject.GetComponent<PlayerGrapplerStateMachine>();
            ParryStateMachine = gameObject.GetComponent<ParryStateMachine>();
            Input = gameObject.GetComponent<PlayerInputController>();
            Actor = gameObject.GetComponent<PlayerActor>();
            AnimManager = GetComponentInChildren<PlayerAnimationStateManager>();
            DeathManager = GetComponentInChildren<PlayerDeathManager>();
            Puncher = GetComponentInChildren<Puncher>();
            
            if (MyGrappleHook == null) throw new ConstraintException("PlayerCore must have GrappleHook");
            if (AnimManager == null) throw new ConstraintException("PlayerCore must have AnimManager");
            //gameObject.AddComponent<PlayerCrystalResponse>();
            //gameObject.AddComponent<PlayerSpikeResponse>();
        }
    }
}