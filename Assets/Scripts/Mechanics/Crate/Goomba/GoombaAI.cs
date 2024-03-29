using System;
using UnityEngine;

namespace Combat
{
    public class GoombaAI : Brain<GoombaInput, GoombaBehavior>
    {
        private int _direction = -1;

        private void FixedUpdate()
        {
            if (input.BeingGrappled()) return;
            
            if (input.GetGroundedStatus())
            {
                behavior.MoveDirection(_direction);
            }

            if (input.WillFallOff(_direction))
            {
                _direction *= -1;
            }
        }
    }
}