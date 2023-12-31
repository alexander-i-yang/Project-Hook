using UnityEngine;

namespace Mechanics
{
    public class ZiplineStateEnd : ZiplineState
    {
        public override void TouchGrapple()
        {
            MySM.Transition<ZiplineStateToStart>();
        }

        public override Vector2 CalculateVelocity() => Vector2.zero;
    }
}