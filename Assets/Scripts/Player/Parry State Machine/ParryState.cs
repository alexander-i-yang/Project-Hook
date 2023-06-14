using ASK.Core;
using ASK.Helpers;

using UnityEditor;
using UnityEngine;

namespace Player
{
    public partial class ParryStateMachine
    {
        public abstract class ParryState : PlayerStateMachine.PlayerState<ParryStateMachine, ParryState, ParryStateInput>
        {
            public virtual void ParryStarted() {
                print("parry");
            }

            public virtual void OnCollide() {
                
            }

            public virtual Vector2 ProcessCollideHorizontal(Vector2 oldV, Vector2 newV) => newV;
        }
    }
}