using ASK.Core;
using MyBox;
using UnityEngine;

namespace VFX
{
    public abstract class ParticleLauncher : MonoBehaviour
    {
        public Vector2 EditorLaunchV;
        
        public abstract void Launch(Vector2 v, Vector2 position);
    }
}