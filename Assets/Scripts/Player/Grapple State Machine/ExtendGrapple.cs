using ASK.Core;
using Helpers;
using UnityEngine;

namespace Player
{
    public partial class GrappleStateMachine
    {
        public class ExtendGrapple : GrappleState
        {
            private float _grappleDuration;
            private float _prevTimeScale;
            // private GameTimer2 _timescaleTimer;
            
            public override void Enter(GrappleStateInput i) {
                _grappleDuration = 0;
                smActor.ResetMyGrappleHook();
                _prevTimeScale = Game.TimeManager.TimeScale;
                Input.CurGrappleExtendPos = smActor.transform.position;

                // _timescaleTimer = GameTimerManager.Instance.StartTimer(
                //     MyCore.GrappleBulletTimeDuration,
                //     () => { ResetTimeScale(); print("Timer done");},
                //     // ResetTimeScale,
                //     IncrementType.FIXED_UPDATE
                // );
                Game.TimeManager.SetTimeScale(MyCore.GrappleBulletTimeScale);
            }

            public override void Exit(GrappleStateInput i) {
                base.Exit(i);
                ResetTimeScale();
                // GameTimerManager.Instance.RemoveTimer(_timescaleTimer);
                Input.CurrentGrapplePos = Input.CurGrappleExtendPos;
            }

            private void ResetTimeScale() 
            {
                Game.TimeManager.SetTimeScale(_prevTimeScale);
            }
            
            public override void FixedUpdate()
            {
                base.FixedUpdate();
                _grappleDuration += Game.TimeManager.FixedDeltaTime;
                var updateData = smActor.GrappleExtendUpdate(_grappleDuration, MySM.GetGrappleInputPos());
                Input.CurGrappleExtendPos = updateData.curPoint;
                if (updateData.attachedTo != null)
                {
                    Input.AttachedTo = updateData.attachedTo;
                    MySM.Transition<Grappling>();
                }
            }

            public override void GrappleStarted() {}

            public override void GrappleFinished() {
                MySM.Transition<Idle>();
            }

            public override void CollideHorizontal()
            {
                if (MyCore.GrappleCollideWallStop)
                {
                    MySM.Transition<Idle>();
                }
                base.CollideHorizontal();
            }
            public override void CollideVertical()
            {
                if (MyCore.GrappleCollideWallStop)
                {
                    MySM.Transition<Idle>();
                }
                base.CollideVertical();
            }
        }
    }
}