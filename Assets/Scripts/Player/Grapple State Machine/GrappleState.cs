using A2DK.Phys;
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

            public virtual Vector2 ProcessCollideHorizontal(Vector2 oldV, Vector2 newV) => newV;

            public virtual Vector2 MoveX(PlayerActor p, Vector2 velocity, int direction) => velocity;

            public virtual void Ride(Vector2 v) {}

            public virtual PhysObj ResolveRidingOn(PhysObj p) => p;
        }
    }
}