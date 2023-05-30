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
[RequireComponent(typeof(AbilityStateMachine))]
[RequireComponent(typeof(PlayerCore))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerActor : Actor, IFilterLoggerTarget {
    [SerializeField, AutoProperty(AutoPropertyMode.Parent)] private PlayerStateMachine _stateMachine;
    [SerializeField, AutoProperty(AutoPropertyMode.Parent)] private AbilityStateMachine _abilityStateMachine;
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
    public void UpdateMovementX(int moveDirection, int acceleration, int resistance) {
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
        velocityX = Mathf.MoveTowards(velocityX, targetVelocityX, accel);
    }

    public void Land()
    {
        OnLand?.Invoke(transform.position + Vector3.down * 5.5f);
        _stateMachine.SetGrounded(true, IsMovingUp);
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

    public (Vector2 curPoint, bool hit) GrappleExtendUpdate(float grappleDuration, Vector2 grapplePoint) {
        var ret = (curPoint: Vector2.zero, hit: false);
        Vector2 grappleOrigin = transform.position;
        float dist = _core.GrappleExtendSpeed * grappleDuration;
        Vector2 curPos = (Vector2) transform.position;
        Vector2 dir = (grapplePoint - curPos).normalized;
        RaycastHit2D hit = Physics2D.Raycast(
            grappleOrigin, 
            dir,
            dist,
            LayerMask.GetMask("Ground", "Interactable")
        );

        Vector2 newPoint = curPos + dir * dist;
        ret.curPoint = newPoint;
        

        if (hit.collider != null) {
            PhysObj p = hit.collider.GetComponent<PhysObj>();
            if (p != this) {
                ret.hit = true;
                ret.curPoint = hit.point;
            };
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
        Vector2 newV = velocity - projection; // Get the component of velocity that's orthogonal to the grapple
        if (Vector2.Dot(projection, rawV) >= 0) {
            return;
        }
        velocity = newV.normalized * velocity.magnitude;
    }

    public void PullGrappleUpdate(Vector2 gPoint, float warmPercent) {
        Vector2 rawV = gPoint - (Vector2) transform.position;
        Vector2 targetV = rawV.normalized * _core.MaxGrappleSpeed;
        velocity = Vector2.Lerp(velocity, targetV, _core.GrappleLerpPercent);
        Fall();
    }

    public void GrappleUpdate(Vector2 gPoint, float warmPercent)
    {
        // Fall();
        Vector2 rawV = gPoint - (Vector2) transform.position;
        Vector2 projection = Vector3.Project(velocity, rawV);
        Vector2 newV = velocity - projection; // Get the component of velocity that's orthogonal to the grapple
        // Vector2 newV = ortho.normalized * velocity.magnitude;
        
        
        //If ur moving towards the grapple point, just use that velocity
        if (Vector2.Dot(projection, rawV) >= 0) {
            // newV = (newV + projection);
            return;
        }

        // velocity = newV.normalized * velocity.magnitude;
        velocity = newV.normalized * (projection.magnitude * _core.GrappleNormalMult + newV.magnitude * _core.GrappleOrthMult);
    }

    public void GrappleBoost(Vector2 gPoint) {
        // Vector2 rawV = gPoint - (Vector2) transform.position;
        // Vector2 projection = Vector3.Project(velocity, rawV);
        // Vector2 ortho = velocity - projection;
        
        // velocity += ortho * _core.GrappleBoost;
        // if (velocityY <= 0) return;
        int vxSign = (int) Mathf.Sign(velocityX);
        Vector2 addV = new Vector2(Facing, 1) * _core.GrappleBoost * velocity.magnitude;
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
            if (direction.x != 0) {
                _abilityStateMachine.CollideHorizontal();
                HitWall((int)direction.x);
            } else if (direction.y != 0) {
                _abilityStateMachine.CollideVertical();
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
        velocityY = Math.Min(_core.BonkHeadV, velocityY);
    }

    private void HitWall(int direction) {
        // _abilityStateMachine.HitWall();
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

    public void CollideHorizontalGrapple() {
        // velocity = Vector2.up * velocity.magnitude;
        velocity += new Vector2(0, velocityX * _core.HitWallGrappleMult);
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
        Handles.Label(transform.position, $"Velocity: <{velocityX}, {velocityY}>");
    }
    #endif

    public LogLevel GetLogLevel()
    {
        return LogLevel.Error;
    }
}