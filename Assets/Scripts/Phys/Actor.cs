using System;
using ASK.Core;
using MyBox;
using UnityEngine;

namespace A2DK.Phys {
    public abstract class Actor : PhysObj
    {
        [SerializeField, Foldout("Gravity")] protected int GravityDown;
        [SerializeField, Foldout("Gravity")] protected int GravityUp;
        [SerializeField, Foldout("Gravity")] protected int MaxFall;
        public bool IsMovingUp => velocityY >= 0;

        // private void Awake()
        // {
        //     JostleBehavior = GetComponent<JostleBehavior>();
        // }

        /// <summary>
        /// Moves this actor a specified number of pixels.
        /// </summary>
        /// <param name="direction"><b>MUST</b> be a cardinal direction with a <b>magnitude of one.</b></param>
        /// <param name="magnitude">Must be <b>non-negative</b> amount of pixels to move.</param>
        /// <param name="onCollide">Collision function that determines how to behave when colliding with an object</param>
        /// <returns>True if it needs to stop on a collision, false otherwise</returns>
        public override bool MoveGeneral(Vector2 direction, int magnitude, Func<PhysObj, Vector2, bool> onCollide) {
            if (magnitude < 0) throw new ArgumentException("Magnitude must be >0");

            int remainder = magnitude;
            // If the actor moves at least 1 pixel, Move one pixel at a time
            while (remainder > 0) {
                bool collision = CheckCollisions(direction, onCollide);
                if (collision) {
                    return true;
                }
                transform.position += new Vector3((int)direction.x, (int)direction.y, 0);
                NextFrameOffset += new Vector2((int)direction.x, (int)direction.y);;
                remainder--;
            }
            
            return false;
        }

        public virtual void Fall() {
            velocityY = Math.Max(MaxFall, velocityY + EffectiveGravity() * Game.TimeManager.FixedDeltaTime);
        }

        public bool FallVelocityExceedsMax()
        {
            return velocityY < MaxFall;
        }

        protected int EffectiveGravity()
        {
            return (velocityY > 0 ? GravityUp : GravityDown);
        }
        
        public bool IsGrounded() {
            return CheckCollisions(Vector2.down, (p, d) => {
                return p.IsGround(this);
            });
        }

        public void FlipGravity()
        {
            GravityDown *= -1;
            GravityUp *= -1;
        }

        protected void ApplyVelocity(Vector2 v)
        {
            if (v != Vector2.zero) print(gameObject.name + " apply v " + v);
            velocity += v;
        }

        #region Jostling
        protected PhysObj ridingOn { get; private set; }
        //Prev velocity of RidingOn
        protected Vector2 prevRidingV { get; private set; }
        protected PhysObj prevRidingOn { get; private set; }
        public virtual bool IsRiding(Solid solid) => ridingOn == solid;
        public virtual void Ride(Vector2 direction) => Move(direction);
        
        /**
         * When there was a floor but now there's not
         */
        protected virtual bool JumpedOff() => prevRidingOn != null && ridingOn == null;
        
        /**
         * When the floor was moving but now it's not
         */
        protected virtual bool FloorStopped() => prevRidingV != Vector2.zero && ridingOn != null && ridingOn.velocity == Vector2.zero;
        
        protected virtual bool ShouldApplyV() => JumpedOff() || FloorStopped();
        
        /**
         * Set _ridingOn to whatever CalcRiding returns.
         * Should get called every frame.
         */
        public virtual Vector2 ResolveJostle()
        {
            Vector2 ret = Vector2.zero;
            ridingOn = RidingOn();
            if (ShouldApplyV())
            {
                ret = ResolveApplyV(ret);
            }
            prevRidingOn = ridingOn;
            prevRidingV = ridingOn == null ? Vector2.zero : ridingOn.velocity;
            return ret;
        }
        
        /**
         * Input previousApplyVelocity, output new apply velocity.
         * Only called when shouldApplyV.
         */
        protected virtual Vector2 ResolveApplyV(Vector2 v) => prevRidingV;
        
        public void Push(Vector2 direction, Solid pusher)
        {
            MoveGeneral(direction, 1, (ps, ds) => {
                if (ps != pusher) return Squish(ps, ds);
                return false;
            });
        }
        
        #endregion
    }
}