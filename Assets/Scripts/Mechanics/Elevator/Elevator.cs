using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Elevator destination;

    public Elevator GetDestination() => destination;
}