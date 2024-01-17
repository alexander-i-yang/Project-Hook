using System;
using Mechanics;
using UnityEngine;
using VFX;

namespace Player
{
    
    [RequireComponent(typeof(Circle))]
    public class GrappleCircle : MonoBehaviour
    {
        private Circle _circle;

        [SerializeField] private Transform player;

        [SerializeField] private GrapplerStateMachine _gsm;

        private void Awake()
        {
            _circle = GetComponent<Circle>();
        }

        void FixedUpdate()
        {
            if (_gsm.IsGrappling() || _gsm.IsGrappleExtending())
            {
                _circle.ShouldDraw(true);
                var grapplePos = _gsm.CurGrapplePos();
                _circle.Radius = (grapplePos - (Vector2)player.position).magnitude;
                _circle.transform.position = grapplePos;
            }
            else
            {
                _circle.ShouldDraw(false);
            }
        }
    }
}