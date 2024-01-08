using A2DK.Phys;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace VFX
{
    public class Scarf : MonoBehaviour
    {
        [SerializeField] private int numPoints;
        [SerializeField] private Actor playerTransform;
        [SerializeField] private GameObject _scarfPointPrefab;
        [SerializeField] private float scarfSpacing;

        public void DestroyChildren()
        {
            int n = transform.childCount;
            for (int i = n-1; i >= 0; --i)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        
        public void InitScarfPoints()
        {
            DestroyChildren();
            
            for (int n = 0; n < numPoints; ++n)
            {
                var prevChild = n == 0 ? playerTransform.transform : transform.GetChild(n-1);
                var curChild = Instantiate(_scarfPointPrefab, transform);
                
                curChild.transform.localPosition = new Vector3(0, -n*scarfSpacing, 0);
                
                var scarfPoint = curChild.GetComponent<ScarfPoint>();
                scarfPoint.SetAttachedTo(prevChild.transform);
                scarfPoint.SetSpacing(scarfSpacing);
                #if UNITY_EDITOR
                EditorUtility.SetDirty(curChild);
                #endif
            }
            
            #if UNITY_EDITOR
            EditorUtility.SetDirty(gameObject);
            #endif
        }
    }
}