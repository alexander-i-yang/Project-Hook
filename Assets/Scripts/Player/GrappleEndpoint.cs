using System;
using UnityEngine;

namespace Player
{
    public class GrappleEndpoint : MonoBehaviour
    {
        private PlayerCore _pCore;

        private void Awake()
        {
            _pCore = GetComponentInParent<PlayerCore>();
            transform.SetParent(null);
        }

        private void FixedUpdate()
        {
            transform.position = _pCore.GrapplerStateMachine.CurGrapplePos();
        }

        public bool OutsideOfScreen()
        {
            return Vector2.Distance(transform.position, _pCore.transform.position) > 100;
        }
    }
}