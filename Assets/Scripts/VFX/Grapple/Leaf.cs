using UnityEngine;

namespace VFX
{
    public class Leaf : MonoBehaviour
    {
        public void SetEnabled(bool b) => gameObject.SetActive(b);

        public void SetRotation(float angle)
        {
            transform.rotation = Quaternion.Euler(0, 0 ,angle);
        }
    }
}