using ASK.Core;
using ASK.Helpers;

using UnityEditor;
using UnityEngine;

namespace Player
{
    public partial class GrappleStateMachine
    {
        public abstract class GrappleState : PlayerStateMachine.PlayerState<GrappleStateMachine, GrappleState, GrappleStateInput>
        {
            public virtual void GrappleStarted() {
                
            }

            public virtual void GrappleFinished()
            {
                
            }

            public virtual void CollideHorizontal()
            {
                
            }

            public virtual void CollideVertical()
            {
                
            }

            public virtual Vector2 MoveX(PlayerActor p, Vector2 velocity, int direction) => velocity;
        }
    }
}