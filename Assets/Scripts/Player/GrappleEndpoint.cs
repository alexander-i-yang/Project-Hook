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
        }

        private void FixedUpdate()
        {
            transform.position = _pCore.GrapplerStateMachine.CurrInput.CurGrappleExtendPos;
        }
    }
}