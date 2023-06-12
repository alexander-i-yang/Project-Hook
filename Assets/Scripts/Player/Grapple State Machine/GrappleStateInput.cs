using System.Collections.Generic;
using ASK.Core;
using ASK.Helpers;
using Mechanics;
using UnityEngine;

namespace Player
{   
    public class GrappleStateInput : PlayerStateInput {
        //Grapple
        public Vector2 currentGrapplePos;
        public Vector2 curGrappleExtendPos;
    }
}