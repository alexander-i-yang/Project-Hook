using System;
using System.Collections;
using System.Collections.Generic;
using Mechanics;
using UnityEngine;

namespace Mechanics
{
    public class ElevatorOut : Elevator
    {
        [SerializeField] private GameObject walls;
        private Animator _animator;
        private SpriteRenderer _sr;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _sr = GetComponent<SpriteRenderer>();
        }

        void Start()
        {
            walls.SetActive(false);
        }
        
        private void OnTriggerEnter2D(Collider2D other) {
            // Add logic here to check if the player has eliminated all entities!!!!!!!!!!!!!!
            if (other.GetComponent<OnElevatorEnter>() is { } e)
            {
                _animator.Play("Close");
                e.OnEnter(this);
                _sr.sortingLayerName = "VFX";
                walls.SetActive(true);
            }
        }
    }
}