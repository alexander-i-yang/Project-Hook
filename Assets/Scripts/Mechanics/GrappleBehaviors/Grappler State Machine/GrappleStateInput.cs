using System.Collections.Generic;
using A2DK.Phys;
using ASK.Core;
using ASK.Helpers;
using Mechanics;
using Phys.PhysObjStateMachine;
using UnityEngine;

namespace Mechanics
{   
    public class GrappleStateInput : PhysObjStateInput {
        //Grapple
        public Vector2 CurrentGrapplePos;
        public Vector2 CurGrappleExtendPos;
        public IGrappleable AttachedTo;
        public PhysObj AttachedToPhysObj => AttachedTo.GetPhysObj();
    }
}