using ASK.Core;

namespace Player
{
    public partial class ParryStateMachine
    {
        public class ParryAiming : ParryState
        {
            private Timescaler.TimeScale _timescale;

            public override void Enter(ParryStateInput i)
            {
                _timescale = Game.TimeManager.ApplyTimescale(MySM.BulletTimeScale, 2);
            }

            public override void Exit(ParryStateInput i) {
                Game.TimeManager.RemoveTimescale(_timescale);
            }
            
            public override void FixedUpdate()
            {
                Input.CurAimPos = MySM.GetAimInputPos();
                MySM.MyCore.Puncher.SetAim(Input.CurAimPos);
            }

            public override void ReadParryInput(bool parryInput)
            {
                if (!parryInput) MySM.Transition<Parrying>();
            }
        }
    }
}