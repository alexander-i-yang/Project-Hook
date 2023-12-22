using UnityEngine;

class SetViewportRect : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 0.5f);
    }
}