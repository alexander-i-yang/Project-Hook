using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private GameObject destination;

    public Vector3 GetDestination()
    {
        return destination.transform.position;
    }
}