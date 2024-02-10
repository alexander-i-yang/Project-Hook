using System;
using UnityEngine;

namespace Combat
{
    //This class goes on objects that block enemies' line of sight.
    public class SightBlocker : MonoBehaviour
    {
        public bool CanSeeThrough(Vector2 direction) => true;
    }
}