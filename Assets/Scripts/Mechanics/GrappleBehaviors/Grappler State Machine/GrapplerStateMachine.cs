using System;
using A2DK.Phys;
using ASK.Core;
using ASK.Helpers;
using Helpers;
using Phys.PhysObjStateMachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Mechanics
{
    public abstract partial class GrapplerStateMachine : PhysObjStateMachine<GrapplerStateMachine, GrapplerStateMachine.GrappleState, GrappleStateInput, Actor> {
        [Tooltip("Grapple Stops when you collide with a wall")]
        [SerializeField] public bool GrappleCollideWallStop;
        
        //Expose to inspector
        public UnityEvent<GrapplerStateMachine> OnAbilityStateChange;
        public UnityEvent OnGrappleAttach;
        public UnityEvent OnGrappleDetach;
        
        [Tooltip("Grapple extend units per second")]
        [SerializeField] public float GrappleExtendSpeed;
        
        [SerializeField] public float GrappleBulletTimeScale;

        [Tooltip("Start grapple energy loss")]
        [SerializeField] public float GrappleStartMult;

        [Tooltip("Multiplier for magnitude of normal component of velocity")]
        [SerializeField] public float GrappleNormalMult;

        [Tooltip("Multiplier for magnitude of ortho component of velocity")]
        [SerializeField] public float GrappleOrthMult;
        
        [Tooltip("Angle from the vertical that it takes to slow down a grapple")]
        [SerializeField] public float SmallAngle;

        [Tooltip("Magnitude that u need to slow down a small angle grapple")]
        [SerializeField] public float SmallAngleMagnitude;

        [Tooltip("Angle that u need to be at to zero out your velocity")]
        [SerializeField] public float ZeroAngle;
        
        [Tooltip("Boost speed multiplier after leaving the grapple")]
        [SerializeField] public float GrappleBoostSpeed;
        
        [Tooltip("Min boost speed")]
        [SerializeField] public float GrappleMinBoost;
        
        [Tooltip("Max boost speed")]
        [SerializeField] public float MaxGrappleBoostSpeed;

        [Tooltip("Max grapple distance in the x and y directions.")]
        [SerializeField] public Vector2 MaxGrappleDistance;
        
        #region Overrides
        protected override void SetInitialState() 
        {
            SetState<Idle>();
        }

        protected override void Init()
        {
            base.Init();
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
            
            if (GrappleStarted())
            {
                CurrState.GrappleStarted();
            }

            if (GrappleFinished())
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

        public Vector2 ProcessMoveX(Vector2 velocity, int direction) {
            return CurrState.MoveX(velocity, direction);
        }

        public virtual Vector2 ProcessCollideHorizontal(Vector2 oldV, Vector2 newV) => CurrState.ProcessCollideHorizontal(oldV, newV);

        #endregion

        public bool IsGrappling() => IsOnState<Swinging>() || IsOnState<Pulling>();

        public bool IsGrappleExtending() => IsOnState<ExtendGrapple>();

        // public Vector2 GetGrappleInputPos() => MyCore.Input.GetAimPos(MyPhysObj.transform.position);
        public abstract Vector2 GetGrappleInputPos();

        public Vector2 GetGrapplePos() => CurrInput.CurrentGrapplePos;

        public Vector2 GetGrappleExtendPos() => CurrInput.CurGrappleExtendPos;

        public Vector2 ResolveRide(Vector2 v) => CurrState.ResolveRide(v);
        public PhysObj CalcRiding(PhysObj p) => CurrState.ResolveRidingOn(p);

        public void Push(Vector2 direction, PhysObj pusher) => CurrState.Push(direction, pusher);

        protected void StartGrapple(Vector2 gPoint)
        {
            Vector2 velocity = MyPhysObj.velocity;
            Vector2 rawV = gPoint - (Vector2) transform.position;
            Vector2 projection = Vector3.Project(velocity, rawV);
            Vector2 ortho = velocity - projection; // Get the component of velocity that's orthogonal to the grapple
            if (Vector2.Dot(projection, rawV) >= 0) {
                return;
            }
            // velocity = ortho.normalized * velocity.magnitude * _core.GrappleStartMult;
            velocity = ortho.normalized * (Mathf.Lerp(ortho.magnitude, velocity.magnitude, GrappleStartMult));
            MyPhysObj.SetVelocity(velocity);
        }
        
        protected void GrappleUpdate(Vector2 gPoint, float warmPercent)
        {
            Vector2 velocity = MyPhysObj.velocity;
            Vector2 rawV = gPoint - (Vector2) transform.position;
            Vector2 projection = Vector3.Project(velocity, rawV);
            Vector2 ortho = velocity - projection; // Get the component of velocity that's orthogonal to the grapple
        
            //If ur moving towards the grapple point, just use that velocity
            if (Vector2.Dot(projection, rawV) >= 0) {
                return;
            }
            velocity = ortho.normalized * (projection.magnitude * GrappleNormalMult + ortho.magnitude * GrappleOrthMult);

            float angle = Vector2.Angle(rawV, Vector2.up);
            if (velocity.magnitude < SmallAngleMagnitude && angle <= SmallAngle) {
                if (Math.Sign(rawV.x) == Math.Sign(velocity.x)) {
                    float newMag = Helpers.Helpers.ClosestBetween(-SmallAngleMagnitude, SmallAngleMagnitude, (ortho + projection).magnitude);
                    velocity = ortho.normalized * newMag;
                }
                if (angle < ZeroAngle) {
                    velocity *= 0.25f;
                }
            }
            MyPhysObj.SetVelocity(velocity);
        }
        
        protected abstract Vector2 CollideHorizontalGrapple();

        protected abstract bool GrappleStarted();
        protected abstract bool GrappleFinished();
        protected abstract void CollideVerticalGrapple();
        public void GrappleBoost() {
            int vxSign = (int) Mathf.Sign(MyPhysObj.velocityX);
            Vector2 addV = new Vector2(MyPhysObj.Facing, 1) * Mathf.Max(GrappleBoostSpeed * MyPhysObj.velocity.magnitude, GrappleMinBoost);
            addV = addV.normalized * Mathf.Clamp(addV.magnitude, -MaxGrappleBoostSpeed, MaxGrappleBoostSpeed);
            MyPhysObj.ApplyVelocity(addV);
        }
        protected abstract Vector2 MoveXGrapple(Vector2 velocity, Vector2 inputCurrentGrapplePos, int direction);
        protected (Vector2 curPoint, IGrappleable attachedTo, GrappleapleType grappleType) GrappleExtendUpdate(float grappleDuration, Vector2 grapplePoint)
        {
            (Vector2 curPoint, IGrappleable attachedTo, GrappleapleType grappleType) ret = (Vector2.zero, null, GrappleapleType.SWING);
            Vector2 grappleOrigin = transform.position;
            float dist = GrappleExtendSpeed * grappleDuration;
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
                    IGrappleable p = hit.collider.GetComponent<IGrappleable>();
                    if (p != null) {
                        var newRet = p.AttachGrapple(MyPhysObj, hit.point);
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
        protected abstract void ResetMyGrappleHook();

        public void BreakGrapple() => CurrState.BreakGrapple();

        private bool GrappleTooFar()
        {
            return false;
            Vector2 d = CurrInput.CurGrappleExtendPos - (Vector2)transform.position;
            d = d.Abs();
            return d.x > MaxGrappleDistance.x || d.y > MaxGrappleDistance.y;
        }
    }
}