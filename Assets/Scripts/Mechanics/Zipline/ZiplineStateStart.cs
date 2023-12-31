using UnityEngine;

namespace Mechanics
{
    public class ZiplineStateStart : ZiplineState
    {
        public override void TouchGrapple()
        {
            MySM.Transition<ZiplineStateToEnd>();
        }

        public override Vector2 CalculateVelocity() => Vector2.zero;
    }
}