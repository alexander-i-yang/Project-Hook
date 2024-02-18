using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public LayerMask groundLayer;
    private bool _grounded = true;
    private int _direction = -1;
    private bool _turnable = false;
    private float _raycastDistance = 8.7f;

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _raycastDistance, groundLayer);
        Debug.DrawRay(transform.position, Vector2.down * _raycastDistance, Color.red);

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

    public bool getGroundedStatus() {
        return _grounded;
    }

    public Vector3 getPositionStatus() {
        return transform.position;
    }
}
