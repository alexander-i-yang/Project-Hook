using UnityEngine;

namespace Mechanics
{
    public class ZiplineStateToStart : ZiplineState
    {
        public override void TouchGrapple()
        {
            MySM.Transition<ZiplineStateToEnd>();
        }

        public override Vector2 CalculateVelocity() => MySM.MyZ.VToStart();

        public override void FixedUpdate()
        {
            if (MySM.MyZ.ReachedStart())
            {
                MySM.MyZ.SetPosStart();
                MySM.Transition<ZiplineStateStart>();
            }
        }
    }
}