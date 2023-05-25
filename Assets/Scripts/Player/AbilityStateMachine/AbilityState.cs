using ASK.Core;
using ASK.Helpers;

using UnityEditor;
using UnityEngine;

namespace Player
{
    public partial class AbilityStateMachine
    {
        public abstract class AbilityState : State<AbilityStateMachine, AbilityState, AbilityStateInput>
        {
            protected PlayerCore core => MySM.MyCore;
            protected PlayerSpawnManager spawnManager => core.SpawnManager;
            protected PlayerAnimationStateManager animManager => core.AnimManager;
            protected PlayerActor smActor => core.Actor;

            public virtual void GrappleStarted() {
                Vector2 mousePos = core.Input.GetMousePos();
                // Vector2 mousePos = (Vector2)smActor.transform.position + new Vector2(smActor.Facing * 2, 1) * 10;
                Vector2? grapplePoint = smActor.GetGrapplePoint(mousePos);
                if (grapplePoint != null)
                {
                    Input.currentGrapplePos = grapplePoint.Value;
                    MySM.Transition<ExtendGrapple>();
                }
            }

            public virtual void GrappleFinished()
            {
                
            }

            #if UNITY_EDITOR
            protected void OnDrawGizmosSelected() {
             
                Handles.DrawLine(smActor.transform.position, Input.currentGrapplePos);
            }
            #endif
        }
    }
}