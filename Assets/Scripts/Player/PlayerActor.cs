using System;
using System.Collections;
using ASK.Core;
using ASK.Helpers;
using A2DK.Phys;
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
    private GrappleStateMachine _grappleStateMachine => _core.GrappleStateMachine;
    [SerializeField, AutoProperty(AutoPropertyMode.Parent)] private BoxCollider2D _collider;
    [SerializeField] private SpriteRenderer sprite;

    private bool _hitWallCoroutineRunning;
    private float _hitWallPrevSpeed;

    public int Facing => sprite.flipX ? -1 : 1;    //-1 is facing left, 1 is facing right

    [Foldout("Movement Events", true)]
    public ActorEvent OnJumpFromGround;
    public ActorEvent OnDoubleJump;
    public UnityEvent OnDiveStart;
    public ActorEvent OnDogo;
    
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

    private void FixedUpdate()
    {
        ApplyVelocity(ResolveJostle());

        Vector2 newV = Vector2.zero;
        newV = _movementStateMachine.ProcessMoveX(this, newV, _core.Input.GetMovementInput());
        newV = _grappleStateMachine.ProcessMoveX(this, newV, _core.Input.GetMovementInput());
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

    #region Grapple
    public Vector2? GetGrapplePoint(Vector2 grapplePos)
    {
        Vector2 grappleOrigin = transform.position;
        Vector2 dir = grapplePos - grappleOrigin;
        // RaycastHit2D[] hits = Physics2D.RaycastAll(grappleOrigin, Vector2.right, 1000, LayerMask.NameToLayer("Ground"));
        RaycastHit2D hit = Physics2D.Raycast(
            grappleOrigin, 
            dir.normalized,
            1000f,
            LayerMask.GetMask("Ground", "Interactable")
        );
        if (hit.collider == null) return null;
        
        PhysObj p = hit.collider.GetComponent<PhysObj>();
        if (p == null) return null;
        return hit.point;
    }

    public (Vector2 curPoint, IGrappleAble attachedTo) GrappleExtendUpdate(float grappleDuration, Vector2 grapplePoint) {
        var ret = (curPoint: Vector2.zero, attachedTo: (IGrappleAble)null);
        Vector2 grappleOrigin = transform.position;
        float dist = _core.GrappleExtendSpeed * grappleDuration;
        Vector2 curPos = (Vector2) transform.position;
        Vector2 dir = (grapplePoint - curPos).normalized;
        RaycastHit2D[] hits = Physics2D.RaycastAll(
            grappleOrigin, 
            dir,
            dist,
            LayerMask.GetMask("Ground", "Interactable")
        );

        Vector2 newPoint = curPos + dir * dist;
        ret.curPoint = newPoint;

        foreach (var hit in hits) {
            if (hit.collider != null) {
                IGrappleAble p = hit.collider.GetComponent<IGrappleAble>();
                if (p != null) {
                    var newRet = p.GetGrapplePoint(this, hit.point);
                    if (newRet.attachedTo != null) {
                        ret = newRet;
                        
                        break;
                    }
                };
            }
        }
        // if (Mathf.Abs(dir.magnitude) <= dist) ret.hit = true;
        
        // ret.hit = _core.MyGrappleHook.SetPos(newPoint);

        // _core.MyGrappleHook.Move(dir * _core.GrappleExtendSpeed);
        // ret.curPoint = _core.MyGrappleHook.transform.position;
        // if (_core.MyGrappleHook.DidCollide) ret.hit = true;
        return ret;
    }

    public void ResetMyGrappleHook() {_core.MyGrappleHook.Reset(transform.position);}

    public void StartGrapple(Vector2 gPoint) {
        Vector2 rawV = gPoint - (Vector2) transform.position;
        Vector2 projection = Vector3.Project(velocity, rawV);
        Vector2 ortho = velocity - projection; // Get the component of velocity that's orthogonal to the grapple
        if (Vector2.Dot(projection, rawV) >= 0) {
            return;
        }
        // velocity = ortho.normalized * velocity.magnitude * _core.GrappleStartMult;
        velocity = ortho.normalized * (Mathf.Lerp(ortho.magnitude, velocity.magnitude, _core.GrappleStartMult));
    }

    public void PullGrappleUpdate(Vector2 gPoint) {
        Vector2 rawV = gPoint - (Vector2) transform.position;
        Vector2 targetV = rawV.normalized * _core.MaxGrappleSpeed;
        velocity = Vector2.Lerp(velocity, targetV, _core.GrappleLerpPercent);
        Fall();
    }

    //recalculate velocity during grapple
    public void GrappleUpdate(Vector2 gPoint, float warmPercent)
    {
        Vector2 rawV = gPoint - (Vector2) transform.position;
        Vector2 projection = Vector3.Project(velocity, rawV);
        Vector2 ortho = velocity - projection; // Get the component of velocity that's orthogonal to the grapple
        
        //If ur moving towards the grapple point, just use that velocity
        if (Vector2.Dot(projection, rawV) >= 0) {
            return;
        }
        velocity = ortho.normalized * (projection.magnitude * _core.GrappleNormalMult + ortho.magnitude * _core.GrappleOrthMult);

        float angle = Vector2.Angle(rawV, Vector2.up);
        if (velocity.magnitude < _core.SmallAngleMagnitude && angle <= _core.SmallAngle) {
            if (Mathf.Sign(rawV.x) == Mathf.Sign(velocityX)) {
                float newMag = ClosestBetween(-_core.SmallAngleMagnitude, _core.SmallAngleMagnitude, (ortho + projection).magnitude);
                velocity = ortho.normalized * newMag;
            }
            if (angle < _core.ZeroAngle) {
                velocity *= 0.25f;
            }
        }
    }

    public Vector2 MoveXGrapple(Vector2 oldV, Vector2 gPos, int direction) {
        Vector2 rawV = gPos - (Vector2) transform.position;
        Vector2 projection = Vector3.Project(oldV, rawV);
        Vector2 ortho = oldV - projection;
        return direction == 0 ? oldV : ortho * _core.MoveXGrappleMult;
    }

    public float ClosestBetween(float a, float b, float x) {
        if (x <= a || x >= b) return x;
        return x < (b-a)/2 + a ? a : b;
    }

    public void GrappleBoost(Vector2 gPoint) {
        // Vector2 rawV = gPoint - (Vector2) transform.position;
        // Vector2 projection = Vector3.Project(velocity, rawV);
        // Vector2 ortho = velocity - projection;
        
        // velocity += ortho * _core.GrappleBoost;
        // if (velocityY <= 0) return;
        int vxSign = (int) Mathf.Sign(velocityX);
        Vector2 addV = new Vector2(Facing, 1) * Mathf.Max(_core.GrappleBoost * velocity.magnitude, _core.GrappleMinBoost);
        addV = addV.normalized * Mathf.Clamp(addV.magnitude, -_core.MaxGrappleBoostMagnitude, _core.MaxGrappleBoostMagnitude);
        velocity += addV;
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
        return _movementStateMachine.IsOnState<MovementStateMachine.Diving>();
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
        return _movementStateMachine.IsOnState<MovementStateMachine.DogoJumping>();
    }
    
    public bool IsDogoing()
    {
        return _movementStateMachine.IsOnState<MovementStateMachine.Dogoing>();
    }

    public void BallBounce(Vector2 direction)
    {
        if (direction.x != 0)
        {
            velocityX = Math.Sign(direction.x) * -150;
        }
    }
    
    public bool IsDrilling() {
        return _movementStateMachine.UsingDrill;
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
                newV = _grappleStateMachine.ProcessCollideHorizontal(oldV, newV);
                newV = _core.ParryStateMachine.ProcessCollideHorizontal(oldV, newV);
                velocity += newV;
            } else if (direction.y != 0) {
                _grappleStateMachine.CollideVertical();
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

    //TODO: figure out what to do with this
    public void Parry(Vector2 oldV) {
        float prevV = velocity.x;
        velocity = Vector2.left * oldV.x * _core.ParryVMult;
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
        Vector2 ret = _core.GrappleStateMachine.ResolveRide(direction);
        base.Ride(ret);
    }
    
    public override PhysObj RidingOn()
    {
        PhysObj p = base.RidingOn();
        return _grappleStateMachine.CalcRiding(p);
    }

    public override bool Push(Vector2 direction, Solid solid)
    {
        _core.GrappleStateMachine.Push(direction, solid);
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

    public Vector2 CollideHorizontalGrapple() {
        // velocity = Vector2.up * velocity.magnitude;
        return new Vector2(0, Math.Abs(velocityX * _core.HitWallGrappleMult));
    }

    public void CollideVerticalGrapple() {
        velocity += new Vector2(velocityY * _core.HitWallGrappleMult * -Mathf.Sign(velocityX), 0);
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
        Handles.Label(transform.position, $"Velocity: <{(int)velocityX}, {(int)velocityY}>");
    }
    #endif

    public LogLevel GetLogLevel()
    {
        return LogLevel.Error;
    }
}