using UnityEditor;
using UnityEngine;
using VFX;

namespace Editor
{
    [CustomEditor(typeof(ParticleBreaker))]
    public class ParticleBreakerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var script = target as ParticleBreaker;
            if (GUILayout.Button("Launch")) script?.Launch(Vector3.zero);
        }
    }
}