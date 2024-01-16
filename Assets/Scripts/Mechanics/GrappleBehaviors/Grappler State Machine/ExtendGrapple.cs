using ASK.Core;
using Helpers;
using Mechanics;
using UnityEngine;

namespace Mechanics
{
    public partial class GrapplerStateMachine
    {
        public class ExtendGrapple : GrappleState
        {
            private float _grappleDuration;
            private Timescaler.TimeScale _timescale;
            // private GameTimer2 _timescaleTimer;
            
            public override void Enter(GrappleStateInput i) {
                _grappleDuration = 0;
                MySM.ResetMyGrappleHook();
                _timescale = Game.TimeManager.ApplyTimescale(MySM.GrappleBulletTimeScale, 2);
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
                
                var updateData = MySM.GrappleExtendUpdate(_grappleDuration, MySM.GetGrappleInputPos());
                Input.CurGrappleExtendPos = updateData.curPoint;
                
                
                
                Input.AttachedTo = updateData.attachedTo;
                
                if (Input.AttachedTo != null)
                {
                    if (updateData.grappleType == GrappleapleType.SWING) MySM.Transition<Swinging>();
                    if (updateData.grappleType == GrappleapleType.PULL) MySM.Transition<Pulling>();
                    MySM.OnGrappleAttach?.Invoke();
                }
            }

            public override void GrappleStarted() {}

            public override void GrappleFinished() {
                MySM.Transition<Idle>();
            }

            public override void CollideHorizontal()
            {
                if (MySM.GrappleCollideWallStop)
                {
                    MySM.Transition<Idle>();
                }
                base.CollideHorizontal();
            }
            public override void CollideVertical()
            {
                if (MySM.GrappleCollideWallStop)
                {
                    MySM.Transition<Idle>();
                }
                base.CollideVertical();
            }
        }
    }
}