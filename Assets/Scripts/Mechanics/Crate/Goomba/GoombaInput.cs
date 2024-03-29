using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using Mechanics;
using UnityEngine;

public class GoombaInput : MonoBehaviour, IInput
{
    private Crate _grappleable;
    
    public LayerMask groundLayer;
    private bool _grounded = true;
    private int _direction = -1;
    private bool _turnable = false;
    private float _raycastDistance;
    public float _angularRaycastDistance = Mathf.Infinity;
    private RaycastHit2D _boxhit;
    private RaycastHit2D _rightWallHit;
    private RaycastHit2D _leftWallHit;
    private float _raycastOffset = 0.1f;
    Vector2 boxSize;

    void Awake() {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
        boxSize = boxCollider.size;
        _raycastDistance = ((boxCollider.size.y)/2) + 1;
        _grappleable = GetComponent<Crate>();
    }

    public bool BeingGrappled()
    {
        return _grappleable.BeingGrappled;
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _raycastDistance, groundLayer);

        _leftWallHit = Physics2D.Raycast(transform.position, Vector2.left, Mathf.Infinity, groundLayer);

        _rightWallHit = Physics2D.Raycast(transform.position, Vector2.right, Mathf.Infinity, groundLayer);

        _boxhit = Physics2D.BoxCast(transform.position, boxSize, 0f, Vector2.down, _raycastDistance, groundLayer);


        Vector2 fallPointRight = CalculateFallPoint(Vector2.right);
        Vector2 fallPointLeft = CalculateFallPoint(Vector2.left);

        Debug.DrawLine(transform.position, fallPointRight, Color.red);
        Debug.DrawLine(transform.position, fallPointLeft, Color.blue);

        Vector2 closestFallPoint = GetClosestFallPoint(fallPointRight, fallPointLeft);
        // Debug.Log("Closest fall point: " + closestFallPoint);



        if (hit.collider != null)
        {
            _grounded = true;
            _turnable = false;
        } else {
            _grounded = false;
            _turnable = true;
        }   

        if (_turnable) {
            Turn();
        }

        Move();
    }

    public Vector2 CalculateFallPoint(Vector2 direction)
    {
        // Cast a ray in the specified direction to find the nearest point where the player could fall off the elevated ground
        RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3)(direction * _raycastOffset), Vector2.down, _angularRaycastDistance, groundLayer);

        // Calculate the fall point based on the ray hit
        Vector2 fallPoint = hit.collider != null ? hit.point : (Vector2)transform.position + direction * _angularRaycastDistance;

        return fallPoint;
    }

    [SerializeField] private Transform bottomLeft;
    [SerializeField] private Transform bottomRight;
    
    public bool WillFallOff(int direction)
    {
        RaycastHit2D hitLeft = Physics2D.Raycast(bottomLeft.position, Vector2.down, _angularRaycastDistance, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(bottomRight.position, Vector2.down, _angularRaycastDistance, groundLayer);

        if (direction > 0)
        {
            return hitRight.distance - hitLeft.distance > 0.05f;
        }
        else
        {
            return hitLeft.distance - hitRight.distance > 0.05f;
        }
    }

    Vector2 GetClosestFallPoint(Vector2 point1, Vector2 point2)
    {
        // Determine which point is closer to the player
        float distanceToPlayer1 = Vector2.Distance(point1, transform.position);
        float distanceToPlayer2 = Vector2.Distance(point2, transform.position);

        return distanceToPlayer1 < distanceToPlayer2 ? point1 : point2;
    }

    private void Move() {
        transform.Translate(10 * Time.deltaTime, 0, 0);
    }

    private void Turn() {
        _direction *= -1;
        if (_direction == 1 && _turnable) {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            _turnable = false;
        } else if (_direction == -1 && _turnable){
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            _turnable = false;
        }
    }

    public bool GetGroundedStatus() {
        return _grounded;
    }

    public Vector3 getPositionStatus() {
        return transform.position;
    }

    public float getDistanceToGround() {
        return _boxhit.distance;
    }

    public float getrightWallDistance() {
        return _rightWallHit.distance;
    }

    public float getleftWallDistance() {
        return _leftWallHit.distance;
    }
}
