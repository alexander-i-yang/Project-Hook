using System;
using A2DK.Phys;
using ASK.Core;
using UnityEngine;
using Combat;
using static Helpers.Helpers;

namespace Mechanics {
    [RequireComponent(typeof(CrateStateMachine))]
    public class Crate : Actor, IPunchable
    {

        [SerializeField] private float _groundedFrictionAccel;
        public float GroundedFrictionAccel => _groundedFrictionAccel;
        [SerializeField] private float _airborneFrictionAccel;
        public float AirborneFrictionAccel => _airborneFrictionAccel;
        
        private CrateStateMachine _stateMachine;

        private bool _useOutsideForces = true;
        
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

        public PhysObj GetPhysObj() => this;
        public void OnStickyEnter(Collider2D stickyCollider)
        {
            throw new NotImplementedException();
        }

        void FixedUpdate()
        {
            _stateMachine.CurrState.SetGrounded(IsGrounded(), IsMovingUp);
            ApplyVelocity(ResolveJostle());
            velocityX = _stateMachine.ApplyXFriction(velocityX);
            MoveTick();
        }
        
        public override bool OnCollide(PhysObj p, Vector2 direction) {
            bool col = base.OnCollide(p, direction);
            if (col) {
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

        public float ApplyXFriction(float prevXVelocity, float frictionAccel)
        {
            if (!_useOutsideForces) return prevXVelocity;
            
            float accel = frictionAccel;
            accel *= Game.TimeManager.FixedDeltaTime;
            return Mathf.SmoothStep(prevXVelocity, 0f, accel);
        }

        public override void Fall()
        {
            if (_useOutsideForces) base.Fall();
        }

        public bool ReceivePunch(Vector2 v)
        {
            velocity = v;
            return false;
        }

        public void SetUseOutsideForces(bool b) => _useOutsideForces = b;
    }
}