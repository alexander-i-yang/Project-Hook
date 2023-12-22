using ASK.Core;
using ASK.Helpers;

using UnityEngine;
using UnityEngine.Events;        

namespace Player
{
    public abstract partial class PlayerStateMachine<M, S, I> : StateMachine<M, S, I>
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

        #region Overrides
        protected override void Init()
        {
            _core = GetComponent<PlayerCore>();
        }
        #endregion
    }
}