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
            var grappleStateMachine = _pCore.GrapplerStateMachine;
            if (grappleStateMachine.IsGrappleExtending() || grappleStateMachine.IsGrappling())
            {
                transform.position = grappleStateMachine.CurrInput.CurGrappleExtendPos;;
            }
            else
            {
                transform.position = _pCore.transform.position;
            }
        }

        public bool OutsideOfScreen()
        {
            return Vector2.Distance(transform.position, _pCore.transform.position) > 100;
        }
    }
}