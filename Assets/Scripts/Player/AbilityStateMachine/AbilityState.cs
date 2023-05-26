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
                
            }

            public virtual void GrappleFinished()
            {
                
            }

            public virtual void HitWall()
            {
                
            }
        }
    }
}