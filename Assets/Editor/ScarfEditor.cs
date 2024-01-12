using UnityEditor;
using UnityEngine;
using VFX;

namespace Editor
{
    [CustomEditor(typeof(Scarf))]
    public class ScarfEditor : UnityEditor.Editor
    {
        private int _numPoints;
        private int _prevNumPoints;
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var script = target as Scarf;
            
            _numPoints = EditorGUILayout.IntSlider("NumPoints: ", _numPoints, 0, 32);
            if (_numPoints != _prevNumPoints)
            {
                script.DestroyChildrenEditor();
                script.InitScarfPoints(_numPoints);
                
                #if UNITY_EDITOR
                EditorUtility.SetDirty(script);
                #endif
            }
            
            _prevNumPoints = _numPoints;
        }
    }
}