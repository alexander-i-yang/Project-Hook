using System;
using UnityEngine;
using UnityEngine.Events;

namespace VFX
{
    public class PunchAnimator : MonoBehaviour
    {
        public UnityEvent<Vector2, Vector2> OnPunch;

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Punch(Vector2 v)
        {
            _animator.Play("Punch");
            OnPunch?.Invoke(v, transform.position);
        }
    }
}