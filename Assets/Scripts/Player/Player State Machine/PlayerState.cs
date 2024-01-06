using ASK.Core;
using ASK.Helpers;
using Phys.PhysObjStateMachine;
using UnityEditor;
using UnityEngine;

namespace Player
{
    public abstract partial class PlayerStateMachine
    {
        public abstract class PlayerState<M, S, I> : PhysObjStateMachine.PhysObjState<M, S, I, PlayerActor>
            where M : PlayerStateMachine<M, S, I>
            where S : PlayerState<M, S, I>
            where I : PlayerStateInput 
        {
            protected PlayerCore MyCore => MySM.MyCore;
            protected PlayerAnimationStateManager animManager => MyCore.AnimManager;
        }
    }
}