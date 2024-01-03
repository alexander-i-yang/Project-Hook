using System.Collections.Generic;
using A2DK.Phys;
using ASK.Core;
using ASK.Helpers;
using Mechanics;
using UnityEngine;

namespace Player
{   
    public class GrappleStateInput : PlayerStateInput {
        //Grapple
        public Vector2 CurrentGrapplePos;
        public Vector2 CurGrappleExtendPos;
        public IGrappleAble AttachedTo;
        public PhysObj AttachedToPhysObj => AttachedTo.GetPhysObj();
    }
}