using System;
using System.Data;
using UnityEngine;

using MyBox;

using Mechanics;
using UnityEngine.Serialization;

namespace Player
{
    //L: The purpose of this class is to ensure that all player components are initialized properly, and it helps keep all of the player properties in one place.
    [RequireComponent(typeof(PlayerActor))]
    [RequireComponent(typeof(PlayerSpawnManager))]
    [RequireComponent(typeof(PlayerStateMachine))]
    [RequireComponent(typeof(PlayerInputController))]
    [RequireComponent(typeof(PlayerScreenShakeActivator))]
    public class PlayerCore : MonoBehaviour
    {
        #region Player Properties
        [Foldout("Move", true)]
        [SerializeField] public int MoveSpeed;
        // public static int MoveSpeed => Instance.moveSpeed;

        [SerializeField] public int MaxAcceleration;
        // public static int MaxAcceleration => Instance.maxAcceleration;  

        [SerializeField] public int MaxAirAcceleration;
        // public static int MaxAirAcceleration => Instance.maxAirAcceleration;

        [SerializeField] public int MaxDeceleration;
        // public static int MaxDeceleration =>Instance. maxDeceleration;

        [Tooltip("Timer between the player crashing into a wall and them getting their speed back (called a cornerboost)")]
        [SerializeField] public float CornerboostTimer;
        // public static float CornerboostTimer => Instance.cornerboostTimer;

        [Tooltip("Cornerboost speed multiplier")]
        [SerializeField] public float CornerboostMultiplier;
        // public static float CornerboostMultiplier => Instance.cornerboostMultiplier;

        [Foldout("Jump", true)]
        [SerializeField] public int JumpHeight;
        // public static int JumpHeight => Instance.jumpHeight;

        [SerializeField] public int DoubleJumpHeight;
        // public static int DoubleJumpHeight => Instance.doubleJumpHeight;

        [SerializeField] public float JumpCoyoteTime;
        // public static float JumpCoyoteTime => Instance.jumpCoyoteTime;

        [SerializeField] public float JumpBufferTime;
        // public static float JumpBufferTime => Instance.jumpBufferTime;

        [SerializeField, Range(0f, 1f)] public float JumpCutMultiplier;
        // public static float JumpCutMultiplier => Instance.jumpCutMultiplier;

        [Foldout("Dive", true)]
        [SerializeField] public int DiveVelocity;
        // public static int DiveVelocity => Instance.diveVelocity;

        [SerializeField] public int DiveDeceleration;
        /*public static int DiveDeceleration
        {
            get => Instance.diveDeceleration;
            set => Instance.diveDeceleration = value;
        }*/

        [Foldout("Dogo", true)]
        [SerializeField] public float DogoJumpHeight;
        // public static float DogoJumpHeight => Instance.dogoJumpHeight;

        [SerializeField] public float DogoJumpXV;
        // public static float DogoJumpXV => Instance.dogoJumpXV;

        [SerializeField] public int DogoJumpAcceleration;
        // public static int DogoJumpAcceleration => Instance.dogoJumpAcceleration;

        [Tooltip("Time where acceleration/decelartion is 0")]
        [SerializeField] public float DogoJumpTime;
        // public static float DogoJumpTime => Instance.dogoJumpTime;
        
        [Tooltip("Amount of time you need to wait to press jump in order to ultra")]
        [SerializeField] public float UltraTimeDelay;
        // public static float UltraTimeDelay => Instance.ultraTimeDelay;
        
        [Tooltip("Window of time you need to press jump in order to ultra")]
        [FormerlySerializedAs("dogoConserveXVTime")]
        [SerializeField] public float UltraTimeWindow;
        // public static float UltraTimeWindow => Instance.ultraTimeWindow;
        
        [Tooltip("Speed multiplier on the boost you get from ultraing")]
        [FormerlySerializedAs("dogoConserveXVTime")]
        [SerializeField] public float UltraSpeedMult;
        // public static float UltraSpeedMult => Instance.ultraSpeedMult;

        [Tooltip("Debug option to change sprite color to green when u can ultra")]
        [SerializeField] public bool UltraHelper;

        [Tooltip("Time to let players input a direction change")]
        [SerializeField] public float DogoJumpGraceTime;

        [Foldout("Grapple", true)]
        [Tooltip("Time it takes to get to full speed for a grapple")]
        [SerializeField] public float GrappleWarmTime;

        [Tooltip("Max grapple speed")]
        [SerializeField] public float MaxGrappleSpeed;

        [Tooltip("Init grapple speed")]
        [SerializeField] public float InitGrappleSpeed;
        
        [Foldout("RoomTransitions", true)]
        [SerializeField, Range(0f, 1f)] public float RoomTransitionVCutX = 0.5f;
        // public static float RoomTransitionVCutX => Instance.roomTransitionVCutX;

        [SerializeField, Range(0f, 1f)] public float RoomTransitionVCutY = 0.5f;
        // public static float RoomTransistionVCutY => Instance.roomTransitionVCutY;
        
        [SerializeField] public float DeathTime;
        // public static float DeathTime => Instance.deathTime;

        #endregion

        public PlayerStateMachine StateMachine { get; private set; }
        public PlayerInputController Input { get; private set; }
        public PlayerActor Actor { get; private set; }
        public PlayerSpawnManager SpawnManager { get; private set; }
        public PlayerScreenShakeActivator MyScreenShakeActivator { get; private set; }
        [NonSerialized] public PlayerAnimationStateManager AnimManager;

        private void Awake()
        {
            // InitializeSingleton(false); //L: Don't make player persistent, bc then there'll be multiple players OO
            StateMachine = gameObject.GetComponent<PlayerStateMachine>();
            Input = gameObject.GetComponent<PlayerInputController>();
            Actor = gameObject.GetComponent<PlayerActor>();
            SpawnManager = gameObject.GetComponent<PlayerSpawnManager>();
            MyScreenShakeActivator = gameObject.GetComponent<PlayerScreenShakeActivator>();
            AnimManager = GetComponentInChildren<PlayerAnimationStateManager>();
            if (AnimManager == null) throw new ConstraintException("PlayerCore must have AnimManager");
            //gameObject.AddComponent<PlayerCrystalResponse>();
            //gameObject.AddComponent<PlayerSpikeResponse>();
        }
    }
}