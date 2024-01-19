using System;
using UnityEngine;

namespace VFX
{
    public class Lamp : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void TurnRed()
        {
            _animator.Play("Red");
        }
    }
}