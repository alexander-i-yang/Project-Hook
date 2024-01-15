using System.Collections.Generic;
using ASK.Helpers;
using UnityEngine;

namespace VFX
{
    public abstract class PrefabParticle : MonoBehaviour
    {
        public abstract void Launch(Vector2 v, float rotationV);
    }
}