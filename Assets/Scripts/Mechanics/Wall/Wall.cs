using A2DK.Phys;
using Combat;
using UnityEngine;

namespace Mechanics {
    public class Wall : Solid
    {
        public override bool Collidable(PhysObj collideWith) => true;
    }
}