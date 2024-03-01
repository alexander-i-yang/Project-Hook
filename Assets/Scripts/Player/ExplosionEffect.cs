using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using ASK.Core;
using Mechanics;
using UnityEngine;
using ASK.Helpers;

namespace Player
{
    public class ExplosionEffect : MonoBehaviour
    {
        public float ExplosionRadius = 5f;
        public float KnockbackForce = 10f;
        private PlayerCore _core;

        private void Awake()
        {
            _core = GetComponent<PlayerCore>();
        }

        public void Explode()
        {
            // Get all colliders within the explosion radius
            Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius);

            foreach (Collider collider in colliders)
            {
                // Check if the collider has a Rigidbody component
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Calculate the direction from the explosion center to the collider
                    Vector2 direction = (collider.transform.position - transform.position).normalized;

                    // Apply knockback force to the collider
                    _core.Actor.ApplyVelocity(direction * KnockbackForce);
                    Debug.Log("EXPLOSION");
                }
            }
        }
    }
}