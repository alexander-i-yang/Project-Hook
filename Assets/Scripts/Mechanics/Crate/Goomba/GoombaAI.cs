using System;
using UnityEngine;

namespace Combat
{
    public class GoombaAI : Brain<GoombaInput, GoombaBehavior>
    {
        public int Direction { get; private set; }
        [SerializeField] private float moveSpeed;
        
        private void FixedUpdate()
        {
            if (input.BeingGrappled()) return;
            
            if (input.GetGroundedStatus())
            {
                if (Direction > 0 && input.GetRightWallDistance() < 1) Direction = -1;
                else if (Direction <= 0 && input.GetLeftWallDistance() < 1) Direction = 1;
            }

            if (input.WillFallOff(Direction))
            {
                Direction *= -1;
            }
        }

        public float ProcessVelocityX(float velocityX, bool grounded, bool beingGrappled)
        {
            if (!grounded || beingGrappled) return velocityX;
            return Direction * moveSpeed;
        }
    }
}