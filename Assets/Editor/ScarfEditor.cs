using UnityEditor;
using UnityEngine;
using VFX;

namespace Editor
{
    [CustomEditor(typeof(Scarf))]
    public class DoorBakerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var script = target as Scarf;
            if(GUILayout.Button("Set Scarf Points"))
            {
                script.InitScarfPoints();
            }
            if(GUILayout.Button("Destroy Children"))
            {
                script.DestroyChildren();
            }
        }
    }
}