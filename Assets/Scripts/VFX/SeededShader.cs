using UnityEngine;

namespace VFX
{
    public class SeededShader : MonoBehaviour
    {
        [Tooltip("Name of the Material property to set")]
        [SerializeField] private string seedName = "_seed";

        [SerializeField] private float min;
        [SerializeField] private float max;
        
        void Awake()
        {
            var mat = GetComponent<SpriteRenderer>().material;
            mat.SetFloat("_seed", Random.Range(min, max));
        }
    }
}