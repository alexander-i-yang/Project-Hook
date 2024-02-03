using System;
using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool _collidable = true;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_collidable) return;
        var d = other.GetComponent<Damageable>();
        if(d) d.TakeDamage(transform.position);
    }

    public void SetCollideable(bool b) => _collidable = b;
}
