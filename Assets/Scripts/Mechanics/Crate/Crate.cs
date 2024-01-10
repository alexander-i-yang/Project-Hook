using System;
using A2DK.Phys;
using ASK.Core;
using UnityEngine;
using Combat;

namespace Mechanics {
    [RequireComponent(typeof(CrateStateMachine))]
    public class Crate : Actor, IGrappleAble, IPunchable
    {

        [SerializeField] private float minPullV;
        [SerializeField] private float initPullMag;
        [SerializeField] private float grappleLerp;
        [SerializeField] private float _groundedFrictionAccel;
        [SerializeField] private float distanceScale;
        [SerializeField] private float stickyDistance;
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

        public (Vector2 curPoint, IGrappleAble attachedTo) AttachGrapple(Actor p, Vector2 rayCastHit)
        {
            // velocity = p.transform.position - transform.position;
            Vector2 rawV = (p.transform.position - transform.position).normalized * initPullMag;
            Vector2 proj = Vector3.Project(velocity, rawV);
            bool oppositeDir = Vector2.Dot(rawV, proj) < 0;
            velocity = rawV + (oppositeDir ? Vector2.zero : proj);
            return (transform.position, this);
        }

        public PhysObj GetPhysObj() => this;

        public Vector2 ContinuousGrapplePos(Vector2 origPos, Actor grapplingActor)
        {
            Vector2 rawV = grapplingActor.transform.position - transform.position;

            if (rawV.magnitude <= stickyDistance)
            {
                velocity = Vector2.zero;
                Move(rawV);
                return transform.position;
            }
            
            float newMag = rawV.magnitude * distanceScale;
            newMag = Mathf.Max(minPullV, newMag);
            
            Vector2 targetV = rawV.normalized * newMag;
            velocity += Vector2.Lerp(Vector2.zero, targetV, grappleLerp);
            
            return transform.position;
            
            /*var rb = GetComponent<Rigidbody2D>();
            rb.AddForceAtPosition(velocity, transform.position);
            return transform.position;*/
        }

        public GrappleapleType GrappleapleType() => Mechanics.GrappleapleType.PULL;

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

        public override void Land()
        {
            base.Land();
        }

        public float ApplyXFriction(float prevXVelocity, float frictionAccel)
        {
            float accel = frictionAccel;
            accel *= Game.TimeManager.FixedDeltaTime;
            return Mathf.SmoothStep(prevXVelocity, 0f, accel);
        }

        public void ReceivePunch(Vector2 v)
        {
            velocity = v;
        }
    }
}