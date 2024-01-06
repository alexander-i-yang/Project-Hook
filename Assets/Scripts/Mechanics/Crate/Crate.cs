using System;
using A2DK.Phys;
using ASK.Core;
using UnityEngine;

namespace Mechanics {
    [RequireComponent(typeof(CrateStateMachine))]
    public class Crate : Actor, IGrappleAble
    {

        [SerializeField] private float maxPullV;
        [SerializeField] private float grappleLerp;
        [SerializeField] private float _groundedFrictionAccel;
        public float GroundedFrictionAccel => _groundedFrictionAccel;
        [SerializeField] private float _airborneFrictionAccel;
        public float AirborneFrictionAccel => _airborneFrictionAccel;

        private CrateStateMachine _stateMachine;
        
        void Awake()
        {
            _stateMachine = GetComponent<CrateStateMachine>();
        }

        public override bool Collidable(PhysObj collideWith)
        {
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

        public (Vector2 curPoint, IGrappleAble attachedTo) GetGrapplePoint(Actor p, Vector2 rayCastHit)
        {
            // velocity = p.transform.position - transform.position;
            return (transform.position, this);
        }

        public Vector2 ContinuousGrapplePos(Vector2 origPos, Actor grapplingActor)
        {
            Vector2 rawV = grapplingActor.transform.position - transform.position;
            Vector2 targetV = rawV.normalized * (maxPullV + Mathf.Sqrt(grapplingActor.velocity.magnitude));
            velocity = Vector2.Lerp(velocity, targetV, grappleLerp);
            return transform.position;
        }

        public PhysObj GetPhysObj() => this;
        public GrappleapleType GrappleapleType() => Mechanics.GrappleapleType.PULL;

        private void FixedUpdate()
        {
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

        public override void Land()
        {
            _stateMachine.CurrState.SetGrounded(true, IsMovingUp);
        }

        public override bool MoveGeneral(Vector2 direction, int magnitude, Func<PhysObj, Vector2, bool> onCollide)
        {
            if (magnitude != 0)
            {
                bool b;
            }
            return base.MoveGeneral(direction, magnitude, onCollide);
        }

        public float ApplyXFriction(float prevXVelocity, float frictionAccel)
        {
            float accel = frictionAccel;
            accel *= Game.TimeManager.FixedDeltaTime;
            return Mathf.SmoothStep(prevXVelocity, 0f, accel);
        }
    }
}