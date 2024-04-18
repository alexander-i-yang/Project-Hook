using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgent : MonoBehaviour
{
    [SerializeField] Transform target;
    UnityEngine.AI.NavMeshAgent agent;
    Vector3 previousPosition;
    
    private void Start() {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = 20.0f;
        previousPosition = transform.position;
    }

    private void Update() {
        agent.SetDestination(target.position);

        Vector2 velocity = CalculateVelocity();
    }

    private Vector2 CalculateVelocity() {
        Vector3 displacement = transform.position - previousPosition;
        Vector2 velocity = new Vector2(displacement.x, displacement.z) / Time.deltaTime;
        previousPosition = transform.position;

        return velocity;
    }
}
