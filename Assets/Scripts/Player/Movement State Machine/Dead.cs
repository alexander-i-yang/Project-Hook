using ASK.Core;
using ASK.Helpers;
using MyBox;
using UnityEngine;

namespace Player
{
    public partial class MovementStateMachine
    {
        public class Dead : MovementState
        {
            private GameTimer _deathTimer;
            
            public override void Enter(MovementStateInput i)
            {
                //MySM._deathAnim.Trigger();
            }

            public override Vector2 PhysTick(Vector2 velocity, Vector2 newV, int direction)
            {
                return newV;
            }
        }
    }
}