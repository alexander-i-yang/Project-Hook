using UnityEditor;
using UnityEngine;
using VFX;

namespace Editor
{
    [CustomEditor(typeof(ParticleLauncher), true)]
    public class ParticleLauncherEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var script = target as ParticleLauncher;
            if (GUILayout.Button("Launch")) script.Launch(script.EditorLaunchV, script.transform.position);
        }
    }
}