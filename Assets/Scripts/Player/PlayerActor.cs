using System;
using System.Collections;
using ASK.Core;
using ASK.Helpers;
using A2DK.Phys;
using Player;
using World;

using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerStateMachine))]
[RequireComponent(typeof(PlayerCore))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerActor : Actor, IFilterLoggerTarget {
    [SerializeField, AutoProperty(AutoPropertyMode.Parent)] private PlayerStateMachine _stateMachine;
    [SerializeField, AutoProperty(AutoPropertyMode.Parent)] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer sprite;

    private bool _hitWallCoroutineRunning;
    private float _hitWallPrevSpeed;

    public int Facing => sprite.flipX ? -1 : 1;    //-1 is facing left, 1 is facing right

    [Foldout("Movement Events", true)]
    public PlayerEvent OnJumpFromGround;
    public PlayerEvent OnDoubleJump;
    public UnityEvent OnDiveStart;
    public PlayerEvent OnDogo;
    public PlayerEvent OnLand;
    
    private Func<Vector2, Vector2> _deathRecoilFunc;

    private float _divePosY;

    private PlayerCore _core;

    private void OnEnable()
    {
        Room.RoomTransitionEvent += OnRoomTransition;
        // EndCutsceneManager.EndCutsceneEvent += OnEndCutscene;
        _core = GetComponent<PlayerCore>();
    }

    private void OnDisable()
    {
        Room.RoomTransitionEvent -= OnRoomTransition;
        // EndCutsceneManager.EndCutsceneEvent -= OnEndCutscene;
    }

    #region Movement
    public void UpdateMovementX(int moveDirection, int acceleration) {
        int targetVelocityX = moveDirection * _core.MoveSpeed;
        int maxSpeedChange = (int) (acceleration * Game.TimeManager.FixedDeltaTime);
        velocityX = Mathf.MoveTowards(velocityX, targetVelocityX, maxSpeedChange);
    }

    public void Land()
    {
        OnLand?.Invoke(transform.position + Vector3.down * 5.5f);
        velocityY = 0;
    }
    #endregion

    #region Jumping

    /// <summary>
    /// Function that bounces the player.
    /// </summary>
    /// <param name="jumpHeight"></param>
    public void Bounce(int jumpHeight)
    {
        velocityY = GetJumpSpeedFromHeight(jumpHeight);
    }

    public void DoubleJump(int jumpHeight, int moveDirection = 0)
    {
        Bounce(jumpHeight);

        // If the player is trying to go in the opposite direction of their x velocity, instantly switch direction.
        if (moveDirection != 0 && moveDirection != Math.Sign(velocityX))
        {
            velocityX = 0;
        }

        OnDoubleJump?.Invoke(transform.position);
    }

    public void JumpFromGround(int jumpHeight)
    {
        Bounce(jumpHeight);

        OnJumpFromGround?.Invoke(transform.position);
    }

    public void JumpCut()
    {
        if (velocityY > 0f)
        {
            velocityY *= _core.JumpCutMultiplier;
        }
    }
    #endregion

    #region Dive
    public void Dive()
    {
        /*if (EndCutsceneManager.IsBeegBouncing)
        {
            velocityY = Mathf.Min(_core.DiveVelocity, velocityY);
        }
        else
        {
            velocityY = _core.DiveVelocity;
        }*/
        velocityY = _core.DiveVelocity;

        _divePosY = transform.position.y;
        OnDiveStart?.Invoke();
    }

    public void UpdateWhileDiving()
    {
        /*if (EndCutsceneManager.IsBeegBouncing)
        {
            velocityY += GravityDown * Game.Instance.FixedDeltaTime;
        }*/
        
        float oldYV = velocityY;
        if (FallVelocityExceedsMax())
        {
            velocityY += _core.DiveDeceleration;
        }
        else
        {
            Fall();
        }
    }

    public bool IsDiving()
    {
        return _stateMachine.IsOnState<PlayerStateMachine.Diving>();
    }
    #endregion

    #region Dogo
    public float Dogo() {
        OnDogo?.Invoke(transform.position);
        float v = velocityX;
        velocityX = 0;
        return v;
    }

    public void DogoJump(int moveDirection, bool conserveMomentum, double oldXV) {
        if (!(moveDirection == 1 || moveDirection == -1)) {
            throw new ArgumentException(
            $"Cannot dogo jump in direction({moveDirection})"
            );
        }
        velocityX = moveDirection * _core.DogoJumpXV;
        if (conserveMomentum)
        {
            float addSpeed = _core.DogoJumpXV * _core.UltraSpeedMult;
            if (moveDirection == 1)
            {
                velocityX = (float)Math.Max(oldXV + addSpeed, _core.DogoJumpXV);
            }
            else if (moveDirection == -1)
            {
                velocityX = (float)Math.Min(oldXV - addSpeed, -_core.DogoJumpXV);
            }
        }

        velocityY = GetJumpSpeedFromHeight(_core.DogoJumpHeight);
    }
    
    public bool IsDogoJumping()
    {
        return _stateMachine.IsOnState<PlayerStateMachine.DogoJumping>();
    }
    
    public bool IsDogoing()
    {
        return _stateMachine.IsOnState<PlayerStateMachine.Dogoing>();
    }

    public void BallBounce(Vector2 direction)
    {
        if (direction.x != 0)
        {
            velocityX = Math.Sign(direction.x) * -150;
        }
    }
    #endregion

    #region  Death

    public bool IsDrilling() {
        return _stateMachine.UsingDrill;
    }

    public void Die(Func<Vector2, Vector2> recoilFunc = null)
    {
        if (recoilFunc == null) recoilFunc = v => v;
        _deathRecoilFunc = recoilFunc;
        _stateMachine.OnDeath();
        // Game.Instance.ScreenShakeManagerInstance.Screenshake(
        //     _core.SpawnManager.CurrentRoom.GetComponentInChildren<CinemachineVirtualCamera>(),
        //     10,
        //     1
        //     );
        _core.MyScreenShakeActivator.ScreenShakeBurst(
            _core.MyScreenShakeActivator.DeathData
        );
    }

    public void DeathRecoil()
    {
        velocity = _deathRecoilFunc(velocity);
    }

    public void DeadStop()
    {
        velocity = Vector2.zero;
    }

    #endregion

    #region Actor Overrides
    public override bool Collidable() {
        return true;
    }

    public override bool OnCollide(PhysObj p, Vector2 direction) {
        if (p != this)
        {
            FilterLogger.Log(this, $"Player collided with object {p} from direction {direction}");
        }
        bool col = p.PlayerCollide(this, direction);
        if (col) {
            if (direction.y > 0) {
                BonkHead();
            }

            if (direction.x != 0) {
                HitWall((int)direction.x);
            }
        }

        return col;
    }

    public override bool PlayerCollide(PlayerActor p, Vector2 direction)
    {
        return false;     
    }

    public override bool IsGround(PhysObj whosAsking)
    {
        return false;
    }

    public override bool Squish(PhysObj p, Vector2 d)
    {
        if (OnCollide(p, d))
        {
            Debug.Log("Squish " + p);
            Die();
        }
        return false;
    }
    #endregion

    public void BonkHead() {
        velocityY = Math.Min(10, velocityY);
    }
    
    public void FloorDisappear()
    {
        if (IsDogoing())
        {
            velocity = Vector2.zero;
            _stateMachine.Transition<PlayerStateMachine.Airborne>();    
        }
    }

    private void HitWall(int direction) {
        if (!_hitWallCoroutineRunning) {
            _hitWallPrevSpeed = velocityX;
            velocityX = 0;
            _hitWallCoroutineRunning = true;
            StartCoroutine(HitWallLogic(direction));
        }
    }

    //Todo: change to fixedUpdate GameTimer
    private IEnumerator HitWallLogic(int direction) {
        for (float t = 0; t < _core.CornerboostTimer; t += Game.TimeManager.FixedDeltaTime) {
            bool movingWithDir = Math.Sign(velocityX) == Math.Sign(direction) || velocityX == 0;
            if (!movingWithDir) {
                break;
            }
            bool stillNextToWall = CheckCollisions(
                Vector2.right * direction, 
                (physObj, d) => {
                return physObj != this && physObj.Collidable();
            });
            if (!stillNextToWall) {
                velocityX = _hitWallPrevSpeed * _core.CornerboostMultiplier;
                break;
            }
            yield return new WaitForFixedUpdate();
        }
        _hitWallCoroutineRunning = false;
    }

    private void OnRoomTransition(Room roomEntering)
    {
        // if (EndCutsceneManager.IsBeegBouncing) return;
        
        velocityX *= _core.RoomTransitionVCutX;
        velocityY *= _core.RoomTransitionVCutY;
    }

    private float GetJumpSpeedFromHeight(float jumpHeight)
    {
        return Mathf.Sqrt(-2f * GravityUp * jumpHeight);
    }

    #if UNITY_EDITOR
    private void OnDrawGizmosSelected() {
        Handles.Label(transform.position, $"Velocity: <{velocityX}, {velocityY}>");
    }
    #endif

    public LogLevel GetLogLevel()
    {
        return LogLevel.Error;
    }
}