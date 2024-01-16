using System;
using System.Collections;
using ASK.Core;
using ASK.Helpers;
using A2DK.Phys;
using Combat;
using Mechanics;
using Player;
using World;
using Helpers;

using MyBox;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerActor : Actor, IFilterLoggerTarget {
    private MovementStateMachine _movementStateMachine => _core.MovementStateMachine;
    private GrapplerStateMachine GrapplerStateMachine => _core.GrapplerStateMachine;
    [SerializeField, AutoProperty(AutoPropertyMode.Parent)] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer sprite;

    private bool _hitWallCoroutineRunning;
    private float _hitWallPrevSpeed;

    [Foldout("Movement Events", true)]
    public ActorEvent OnJumpFromGround;
    public ActorEvent OnDoubleJump;
    public UnityEvent OnDiveStart;
    public ActorEvent OnDogo;
    
    private Func<Vector2, Vector2> _deathRecoilFunc;

    private float _divePosY;

    private PlayerCore _core;

    public override int Facing => sprite.flipX ? -1 : 1;

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

    private void FixedUpdate()
    {
        ApplyVelocity(ResolveJostle());

        Vector2 newV = Vector2.zero;
        newV = _movementStateMachine.ProcessMoveX(this, newV, _core.Input.GetMovementInput());
        newV = GrapplerStateMachine.ProcessMoveX(newV, _core.Input.GetMovementInput());
        velocity += newV;
        MoveTick();
    }

    #region Movement
    public Vector2 CalcMovementX(int moveDirection, int acceleration, int resistance) {
        int vxSign = (int) Mathf.Sign(velocityX);
        // if (Mathf.Abs(smActor.velocityX))
        // int acceleration = moveDirection == vxSign || moveDirection == 0 ? core.AirResistance : core.MaxAirAcceleration;
        
        
        int targetVelocityX = moveDirection * _core.MoveSpeed;
        // float maxSpeedChange = acceleration * Game.TimeManager.FixedDeltaTime;
        // velocityX = Mathf.MoveTowards(velocityX, targetVelocityX, maxSpeedChange);

        float accel = 0;
        if (moveDirection == 0) {
            accel = resistance;
        } else if (moveDirection == vxSign && Mathf.Abs(velocityX) >= Mathf.Abs(targetVelocityX)) {
            accel = resistance/2;
        } else {
            accel = acceleration;
        }

        accel *= Game.TimeManager.FixedDeltaTime;
        return new Vector2(Math.Clamp(targetVelocityX - velocityX, -accel, accel), 0);
        // return new Vector2(Mathf.MoveTowards(velocityX, targetVelocityX, accel), velocityY);
    }

    public override void Land()
    {
        _movementStateMachine.SetGrounded(true, IsMovingUp);
        base.Land();
    }
    #endregion

    #region Jumping

    /// <summary>
    /// Function that bounces the player.
    /// </summary>
    /// <param name="jumpHeight"></param>
    public void Bounce(int jumpHeight)
    {
        float applyV = GetJumpSpeedFromHeight(jumpHeight);
        velocityY = Math.Max(applyV, velocityY + applyV);
    }

    public void DoubleJump(int moveDirection = 0)
    {
        float doubleJumpVelocity = GetJumpSpeedFromHeight(_core.DoubleJumpHeight);
        float addDoubleJumpVelocity = GetJumpSpeedFromHeight(_core.AddDoubleJumpHeight);
        velocityY = Math.Max(doubleJumpVelocity, velocityY + addDoubleJumpVelocity);

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

    #region  Death

    public void Die(Func<Vector2, Vector2> recoilFunc = null)
    {
        if (recoilFunc == null) recoilFunc = v => Vector2.zero;
        _deathRecoilFunc = recoilFunc;
        DeathRecoil();
        _core.DeathManager.Die();
        // Game.Instance.ScreenShakeManagerInstance.Screenshake(
        //     _core.SpawnManager.CurrentRoom.GetComponentInChildren<CinemachineVirtualCamera>(),
        //     10,
        //     1
        //     );
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
    public override bool Collidable(PhysObj collideWith) {
        return collideWith.Collidable(this);
    }

    public override bool OnCollide(PhysObj p, Vector2 direction) {
        if (p != this)
        {
            FilterLogger.Log(this, $"Player collided with object {p} from direction {direction}");
        }
        else
        {
            return false;
        }
        bool col = p.OnCollide(this, direction);
        if (col) {
            if (direction.x != 0) {
                
                Vector2 newV = Vector2.zero;
                Vector2 oldV = velocity;
                newV = HitWall((int)direction.x);
                newV = GrapplerStateMachine.ProcessCollideHorizontal(oldV, newV);
                newV = _core.ParryStateMachine.ProcessCollideHorizontal(oldV, newV);
                velocity += newV;
            } else if (direction.y != 0) {
                GrapplerStateMachine.CollideVertical();
                if (direction.y > 0) {
                    BonkHead();
                }
                if (direction.y < 0) {
                    Land();
                }
            }
        }

        return col;
    }

    public void ParryBounce(Vector2 punchDir)
    {
        punchDir = punchDir.normalized * _core.PunchBounceBoost;
        Vector2 v0 = velocity;
        
        //Split v0 into ortho + normal components
        Vector2 v0n = Vector3.Project(v0, punchDir);
        Vector2 v0o = v0 - v0n;
        velocity = v0o + (Vector2.Dot(punchDir, v0n) <= 0 ? v0n - punchDir : -punchDir);

        // velocity = Helpers.Helpers.CombineVectorsWithReset(velocity, -punchDir.normalized * _core.PunchBounceBoost);
        // velocity += -punchDir.normalized * _core.PunchBounceBoost;
        
        _movementStateMachine.RefreshAbilities();
    }

    /*public override bool PlayerCollide(Actor p, Vector2 direction)
    {
        return false;     
    }*/

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

    #region Jostling
    
    private Vector2 _gracePrevV;
    private GameTimer2 _graceTimer;

    protected override bool FloorStopped()
    {
        if (prevRidingOn == GetBelowPhysObj()) return false;
        return base.FloorStopped();
    }

    protected override Vector2 ResolveApplyV(Vector2 v)
    {
        v = base.ResolveApplyV(v);
        if (_graceTimer != null && GameTimer2.TimerRunning(_graceTimer))
        {
            GameTimerManager.Instance.RemoveTimer(_graceTimer);
            v = _gracePrevV;
        }
        return v;
    }

    protected override bool ShouldApplyV()
    {
        return base.ShouldApplyV() && GrapplerStateMachine.CurrState.ShouldApplyV();
    }

    public override Vector2 ResolveJostle()
    {
        if (base.FloorStopped())
        {
            _gracePrevV = prevRidingV;
            _graceTimer = GameTimerManager.Instance.StartTimer(_core.JostleBoostGraceTime, () => { }, IncrementType.FIXED_UPDATE);
        }
        return base.ResolveJostle();
    }
        
    public override void Ride(Vector2 direction)
    {
        Vector2 ret = _core.GrapplerStateMachine.ResolveRide(direction);
        base.Ride(ret);
    }
    
    public override PhysObj RidingOn()
    {
        PhysObj p = base.RidingOn();
        return GrapplerStateMachine.CalcRiding(p);
    }

    public override bool Push(Vector2 direction, Solid solid)
    {
        _core.GrapplerStateMachine.Push(direction, solid);
        return base.Push(direction, solid);
    }

    #endregion

    private Vector2 HitWall(int direction) {
        // _abilityStateMachine.HitWall();
        if (!_hitWallCoroutineRunning) {
            _hitWallPrevSpeed = velocityX;
            _hitWallCoroutineRunning = true;
            StartCoroutine(HitWallLogic(direction));
        }
        return Vector2.left * velocityX;
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
                return physObj != this && physObj.Collidable(this);
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

    public LogLevel GetLogLevel()
    {
        return LogLevel.Error;
    }
    
    #if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Vector2 v = velocity;
        Puncher p = GetComponentInChildren<Puncher>();
        
        float a = p.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        Vector2 punchV = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
        
        //Split v0 into ortho + normal components
        Vector2 v0n = Vector3.Project(v, punchV);
        Vector2 v0o = v - v0n;
        Vector2 vf = v0o + (Vector2.Dot(punchV, v0n) >= 0 ? v0n : Vector2.zero) - punchV;
        
        Helper.DrawArrow(transform.position, v.normalized * (v.magnitude+8), Color.blue);
        Helper.DrawArrow(transform.position, punchV * 24, Color.red);
        Helper.DrawArrow(transform.position, vf.normalized * (vf.magnitude+8), Color.yellow);
    }
    #endif
}