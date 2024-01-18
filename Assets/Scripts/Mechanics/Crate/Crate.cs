using System;
using A2DK.Phys;
using ASK.Core;
using UnityEngine;
using Combat;
using UnityEngine.Events;

namespace Mechanics {
    [RequireComponent(typeof(CrateStateMachine))]
    public class Crate : Actor, IPunchable
    {

        [SerializeField] private float _groundedFrictionAccel;
        public float GroundedFrictionAccel => _groundedFrictionAccel;
        [SerializeField] private float _airborneFrictionAccel;
        public float AirborneFrictionAccel => _airborneFrictionAccel;
        
        private CrateStateMachine _stateMachine;

        private bool _beingGrappled = false;

        [SerializeField] private float breakVelocity;
        [SerializeField] private UnityEvent<Vector2, Vector2> onBreak;
        [SerializeField] private UnityEvent<Vector2> onPunch;
        
        void Awake()
        {
            _stateMachine = GetComponent<CrateStateMachine>();
        }

        public override bool Collidable(PhysObj collideWith)
        {
            //TODO: :grimmace:
            if (collideWith as Solid)
            {
                return collideWith.Collidable(this);
            }
            else if (collideWith as Actor)
            {
                return false;
            }

            return false;
        }

        public override bool Squish(PhysObj p, Vector2 d)
        {
            Destroy(gameObject);
            return false;
        }

        void FixedUpdate()
        {
            _stateMachine.CurrState.SetGrounded(IsGrounded(), IsMovingUp);
            ApplyVelocity(ResolveJostle());
            velocityX = _stateMachine.ApplyXFriction(velocityX);
            MoveTick();
        }

        public bool ShouldBreak(Vector2 direction, PhysObj against)
        {
            return ((against.velocity - this.velocity) * direction).magnitude >= breakVelocity && !_beingGrappled;
        }

        public override bool OnCollide(PhysObj p, Vector2 direction) {
            bool col = base.OnCollide(p, direction);
            if (p is Crate otherCrate)
            {
                if (!_beingGrappled && otherCrate.ShouldBreak(direction, this))
                {
                    otherCrate.BreakAgainst(this);
                }
            } else if (p is BreakableSolid otherSolid)
            {
                if (otherSolid.ShouldBreak(direction, this))
                {
                    otherSolid.BreakAgainst(this);
                    return false;
                }
            }
            
            if (col) {
                if (ShouldBreak(direction, p))
                {
                    BreakAgainst(p);
                    return true;
                }
                
                if (direction.x != 0) {
                
                    // Vector2 newV = Vector2.zero;
                    // Vector2 oldV = velocity;
                    // newV = HitWall((int)direction.x);
                    velocityX = 0;
                } else if (direction.y != 0) {
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

        private void BreakAgainst(PhysObj p)
        {
            onBreak?.Invoke(p.velocity - this.velocity, transform.position);
            gameObject.SetActive(false);
        }

        public float ApplyXFriction(float prevXVelocity, float frictionAccel)
        {
            if (_beingGrappled) return prevXVelocity;
            
            float accel = frictionAccel;
            accel *= Game.TimeManager.FixedDeltaTime;
            return Mathf.SmoothStep(prevXVelocity, 0f, accel);
        }

        public override void Fall()
        {
            if (!_beingGrappled) base.Fall();
        }

        public bool ReceivePunch(Vector2 v)
        {
            velocity = v;
            onPunch?.Invoke(v);
            return true;
        }

        public void SetBeingGrappled(bool b) => _beingGrappled = b;
    }
}