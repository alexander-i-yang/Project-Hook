using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingOffset : MonoBehaviour
{
    public GameObject navMeshSurfaceGameObject;
    public Vector3 offset = new Vector3(4, 4);

    void Start() {
        navMeshSurfaceGameObject.transform.position += offset;
    }
}
