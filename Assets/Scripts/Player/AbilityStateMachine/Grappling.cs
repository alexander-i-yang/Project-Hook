using ASK.Core;

namespace Player
{
    public partial class AbilityStateMachine
    {
        public class Grappling : AbilityState
        {

            public override void Enter(AbilityStateInput i)
            {
                // _grappleTimer = GameTimer.StartNewTimer(core.GrappleWarmTime);
            }

            public override void FixedUpdate()
            {
                smActor.GrappleUpdate(Input.currentGrapplePos, 0);
                // GameTimer.FixedUpdate(_grappleTimer);
            }

            public override void GrappleFinished()
            {
                base.GrappleFinished();
                MySM.Transition<Idle>();
            }
        }
    }
}