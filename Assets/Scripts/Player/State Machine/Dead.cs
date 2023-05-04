using ASK.Core;
using ASK.Helpers;
using MyBox;
using UnityEngine;

namespace Player
{
    public partial class PlayerStateMachine
    {
        public class Dead : PlayerState
        {
            private GameTimer _deathTimer;
            
            public override void Enter(PlayerStateInput i)
            {
                //MySM._deathAnim.Trigger();
            }
        }
    }
}