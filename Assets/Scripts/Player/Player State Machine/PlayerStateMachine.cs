using ASK.Core;
using ASK.Helpers;
using Phys.PhysObjStateMachine;
using UnityEngine;
using UnityEngine.Events;        

namespace Player
{
    public abstract partial class PlayerStateMachine<M, S, I> : PhysObjStateMachine<M, S, I, PlayerActor>
        where M : PlayerStateMachine<M, S, I>
        where S : PlayerStateMachine.PlayerState<M, S, I>
        where I : PlayerStateInput
    {
        private PlayerCore _core;
        public PlayerCore MyCore
        {
            get
            {
                if (_core == null) _core = GetComponent<PlayerCore>();
                return _core;
            }
        }
    }
}