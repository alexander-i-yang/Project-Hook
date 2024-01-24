using A2DK.Phys;
using ASK.Core;
using ASK.Helpers;
using Phys.PhysObjStateMachine;
using UnityEditor;
using UnityEngine;

namespace Mechanics
{
    public partial class GrapplerStateMachine
    {
        public abstract class GrappleState : PhysObjStateMachine.PhysObjState<GrapplerStateMachine, GrappleState, GrappleStateInput, Actor>
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

            public virtual Vector2 MoveX(Vector2 velocity, int direction) => velocity;

            public virtual Vector2 ResolveRide(Vector2 v) => v;

            public virtual PhysObj ResolveRidingOn(PhysObj p) => p;

            public virtual void Push(Vector2 direction, PhysObj pusher) {}
            public virtual bool ShouldApplyV() => true;

            public virtual void BreakGrapple() => MySM.Transition<Idle>();

            public virtual Vector2 Fall(Vector2 velocity) => velocity;
            public abstract Vector2 PhysTick(Vector2 velocity, Vector2 newV, int getMovementInput);
        }
    }
}