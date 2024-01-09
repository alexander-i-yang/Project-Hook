using ASK.Core;
using Helpers;
using Mechanics;
using UnityEngine;

namespace Player
{
    public partial class GrappleStateMachine
    {
        public class ExtendGrapple : GrappleState
        {
            private float _grappleDuration;
            private Timescaler.TimeScale _timescale;
            // private GameTimer2 _timescaleTimer;
            
            public override void Enter(GrappleStateInput i) {
                _grappleDuration = 0;
                MySM.MyPhysObj.ResetMyGrappleHook();
                _timescale = Game.TimeManager.ApplyTimescale(MyCore.GrappleBulletTimeScale, 2);
                Input.CurGrappleExtendPos = MySM.MyPhysObj.transform.position;

                // _timescaleTimer = GameTimerManager.Instance.StartTimer(
                //     MyCore.GrappleBulletTimeDuration,
                //     () => { ResetTimeScale(); print("Timer done");},
                //     // ResetTimeScale,
                //     IncrementType.FIXED_UPDATE
                // );
            }

            public override void Exit(GrappleStateInput i) {
                base.Exit(i);
                Game.TimeManager.RemoveTimescale(_timescale);
                // GameTimerManager.Instance.RemoveTimer(_timescaleTimer);
                Input.CurrentGrapplePos = Input.CurGrappleExtendPos;
            }
            
            public override void FixedUpdate()
            {
                base.FixedUpdate();
                _grappleDuration += Game.TimeManager.FixedDeltaTime;
                
                var updateData = MySM.MyPhysObj.GrappleExtendUpdate(_grappleDuration, MySM.GetGrappleInputPos());
                Input.CurGrappleExtendPos = updateData.curPoint;
                Input.AttachedTo = updateData.attachedTo;
                
                if (Input.AttachedTo != null)
                {
                    if (Input.AttachedTo.GrappleapleType() == GrappleapleType.SWING) MySM.Transition<Swinging>();
                    if (Input.AttachedTo.GrappleapleType() == GrappleapleType.PULL) MySM.Transition<Pulling>();
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