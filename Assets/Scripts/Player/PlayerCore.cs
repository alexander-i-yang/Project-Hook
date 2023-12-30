using System;
using System.Data;
using UnityEngine;

using MyBox;

using UnityEngine.Serialization;

namespace Player
{
    //L: The purpose of this class is to ensure that all player components are initialized properly, and it helps keep all of the player properties in one place.
    [RequireComponent(typeof(PlayerActor))]
    [RequireComponent(typeof(MovementStateMachine))]
    [RequireComponent(typeof(GrappleStateMachine))]
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

        [Foldout("Jump", true)]
        [SerializeField] public int JumpHeight;

        [SerializeField] public int DoubleJumpHeight;

        [SerializeField] public float JumpCoyoteTime;

        [SerializeField] public float JumpBufferTime;

        [Tooltip("Y velocity after the player hits their head on the ceiling")]
        [SerializeField] public float BonkHeadV;

        [SerializeField, Range(0f, 1f)] public float JumpCutMultiplier;

        [Foldout("Dive", true)]
        [SerializeField] public int DiveVelocity;

        [SerializeField] public int DiveDeceleration;

        [Foldout("Dogo", true)]
        [SerializeField] public float DogoJumpHeight;

        [SerializeField] public float DogoJumpXV;

        [SerializeField] public int DogoJumpAcceleration;

        [Tooltip("Time where acceleration/decelartion is 0")]
        [SerializeField] public float DogoJumpTime;
        
        [Tooltip("Amount of time you need to wait to press jump in order to ultra")]
        [SerializeField] public float UltraTimeDelay;
        
        [Tooltip("Window of time you need to press jump in order to ultra")]
        [FormerlySerializedAs("dogoConserveXVTime")]
        [SerializeField] public float UltraTimeWindow;
        
        [Tooltip("Speed multiplier on the boost you get from ultraing")]
        [FormerlySerializedAs("dogoConserveXVTime")]
        [SerializeField] public float UltraSpeedMult;

        [Tooltip("Debug option to change sprite color to green when u can ultra")]
        [SerializeField] public bool UltraHelper;
        
        [Tooltip("Time to let players input a direction change")]
        [SerializeField] public float DogoJumpGraceTime;

        [Foldout("Grapple", true)]
        
        [Tooltip("Grapple Stops when you collide with a wall")]
        [SerializeField] public bool GrappleCollideWallStop;
        
        [Tooltip("Grapple extend units per second")]
        [SerializeField] public float GrappleExtendSpeed;

        [SerializeField] public float GrappleBulletTimeScale;
       
        [Tooltip("Lerp percent for grapple acceleration")]
        [SerializeField] public float GrappleLerpPercent;

        [Tooltip("Max grapple speed")]
        [SerializeField] public float MaxGrappleSpeed;

        [Tooltip("Start grapple energy loss")]
        [SerializeField] public float GrappleStartMult;

        [Tooltip("Multiplier for magnitude of normal component of velocity")]
        [SerializeField] public float GrappleNormalMult;

        [Tooltip("Multiplier for magnitude of ortho component of velocity")]
        [SerializeField] public float GrappleOrthMult;

        [Tooltip("Boost magnitude multiplier after leaving the grapple")]
        [SerializeField] public float GrappleBoost;
        
        [Tooltip("Min Boost Velocity")]
        [SerializeField] public float GrappleMinBoost;

        [Tooltip("Angle from the vertical that it takes to slow down a grapple")]
        [SerializeField] public float SmallAngle;

        [Tooltip("Magnitude that u need to slow down a small angle grapple")]
        [SerializeField] public float SmallAngleMagnitude;

        [Tooltip("Angle that u need to be at to zero out your velocity")]
        [SerializeField] public float ZeroAngle;

        [Tooltip("Max boost magnitude")]
        [SerializeField] public float MaxGrappleBoostMagnitude;

        [Tooltip("Velocity multiplier from the discarded direction after you it a wall")]
        [SerializeField] public float HitWallGrappleMult;

        [Tooltip("Velocity multiplier for when you move left/right while grappling")]
        [SerializeField] public float MoveXGrappleMult;

        [Foldout("Parry", true)]
        [Tooltip("Velocity mult after parrying a wall")]
        [SerializeField] public float ParryVMult;

        [Tooltip("Time window before hitting a wall")]
        [SerializeField] public float ParryPreCollisionWindow;

        [Tooltip("Time window afteri hitting a wall")]
        [SerializeField] public float ParryPostCollisionWindow;

        
        [Foldout("RoomTransitions", true)]
        [SerializeField, Range(0f, 1f)] public float RoomTransitionVCutX = 0.5f;

        [SerializeField, Range(0f, 1f)] public float RoomTransitionVCutY = 0.5f;
        
        [SerializeField] public float DeathTime;

        #endregion

        public MovementStateMachine MovementStateMachine { get; private set; }
        public GrappleStateMachine GrappleStateMachine { get; private set; }
        public ParryStateMachine ParryStateMachine { get; private set; }
        public PlayerInputController Input { get; private set; }
        public PlayerActor Actor { get; private set; }
        
        public PlayerDeathManager DeathManager { get; private set; }
        
        [NonSerialized] public PlayerAnimationStateManager AnimManager;
        
        private void Awake()
        {
            // InitializeSingleton(false); //L: Don't make player persistent, bc then there'll be multiple players OO
            MovementStateMachine = gameObject.GetComponent<MovementStateMachine>();
            GrappleStateMachine = gameObject.GetComponent<GrappleStateMachine>();
            ParryStateMachine = gameObject.GetComponent<ParryStateMachine>();
            Input = gameObject.GetComponent<PlayerInputController>();
            Actor = gameObject.GetComponent<PlayerActor>();
            AnimManager = GetComponentInChildren<PlayerAnimationStateManager>();
            DeathManager = GetComponentInChildren<PlayerDeathManager>();
            
            if (MyGrappleHook == null) throw new ConstraintException("PlayerCore must have GrappleHook");
            if (AnimManager == null) throw new ConstraintException("PlayerCore must have AnimManager");
            //gameObject.AddComponent<PlayerCrystalResponse>();
            //gameObject.AddComponent<PlayerSpikeResponse>();
        }
    }
}