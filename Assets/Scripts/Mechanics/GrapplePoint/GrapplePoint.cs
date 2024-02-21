using A2DK.Phys;
using UnityEngine;
using System.Collections.Generic;
using Combat;

namespace Mechanics {
    public class GrapplePoint : Solid, IPunchable
    {
        public override bool Collidable(PhysObj collideWith) => false;
        public bool ReceivePunch(Vector2 v) => true;
    }
}