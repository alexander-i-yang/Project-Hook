using System.Data;
using A2DK.Phys;
using ASK.Core;
using UnityEngine;

namespace Mechanics
{
    public partial class GrapplerStateMachine
    {
        
        public class Pulling : GrappleState
        {
            
            public override void Enter(GrappleStateInput i)
            {
                // smActor.StartGrapple(Input.CurrentGrapplePos);
            }

            public override void FixedUpdate()
            {
                if (!Input.AttachedToPhysObj.gameObject.activeSelf) GrappleFinished();
                Input.CurrentGrapplePos = Input.AttachedTo.ContinuousGrapplePos(Input.CurrentGrapplePos, MySM.MyPhysObj);
                base.FixedUpdate();
                // if (_attachedTo.velocity == Vector2.zero && _prevV != Vector2.zero)
                // {
                //     smActor.ApplyVelocity(_prevV);
                // }
                // _prevV = _attachedTo.velocity;
                // GameTimer.FixedUpdate(_grappleTimer);
            }

            public override void GrappleFinished()
            {
                Input.AttachedTo.DetachGrapple();
                MySM.Transition<Idle>();
                MySM.OnGrappleDetach?.Invoke();
                // MyCore.MovementStateMachine.RefreshAbilities();
            }
        }
    }
}