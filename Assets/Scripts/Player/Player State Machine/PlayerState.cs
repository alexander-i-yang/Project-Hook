using ASK.Core;
using ASK.Helpers;

using UnityEditor;
using UnityEngine;

namespace Player
{
    public abstract partial class PlayerStateMachine
    {
        public abstract class PlayerState<M, S, I> : State<M, S, I>
            where M : PlayerStateMachine<M, S, I>
            where S : PlayerStateMachine.PlayerState<M, S, I>
            where I : PlayerStateInput 
        {
            protected PlayerCore MyCore => MySM.MyCore;
            protected PlayerAnimationStateManager animManager => MyCore.AnimManager;
            protected PlayerActor smActor => MyCore.Actor;
        }
    }
}