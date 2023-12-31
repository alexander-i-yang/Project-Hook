using UnityEngine;

namespace Mechanics
{
    public class ZiplineStateToEnd : ZiplineState
    {
        public override void TouchGrapple()
        {
            MySM.Transition<ZiplineStateToStart>();
        }

        public override Vector2 CalculateVelocity() => MySM.MyZ.VToEnd();

        public override void FixedUpdate()
        {
            if (MySM.MyZ.ReachedEnd())
            {
                MySM.MyZ.SetPosEnd();
                MySM.Transition<ZiplineStateEnd>();
            }
        }
    }
}