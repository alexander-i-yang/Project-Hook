using System;
using ASK.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerArmManager : MonoBehaviour
    {
        [SerializeField] private Arm leftArm;
        [SerializeField] private Arm rightArm;

        private PlayerGrapplerStateMachine _inputGrapple;
        private ParryStateMachine _inputParry;
        [SerializeField] private float duration;

        private void Awake()
        {
            _inputGrapple = GetComponentInParent<PlayerGrapplerStateMachine>();
            _inputParry = GetComponentInParent<ParryStateMachine>();
        }

        private void Update()
        {
            leftArm.SetAngle(GetAngleRaw());
            rightArm.SetAngle(GetParryAngle());
        }

        private float GetParryAngle()
        {
            if (_inputParry.IsOnState<Parrying>() || _inputParry.IsOnState<ParryStateMachine.ParryAiming>())
            {
                Vector2 aimPos = _inputParry.GetAimInputPos() - (Vector2)transform.position;
                return Mathf.Atan2(aimPos.y, aimPos.x) * Mathf.Rad2Deg + 180;
            }

            return 90;
        }

        float GetAngleRaw()
        {
            float ret;
            if (_inputGrapple.IsGrappling() || _inputGrapple.IsGrappleExtending())
            {
                Vector2 aimPos = _inputGrapple.CurGrapplePos() - (Vector2)transform.position;
                ret = Mathf.Atan2(aimPos.y, aimPos.x) * Mathf.Rad2Deg;
                return ret;
            }

            return 270;
        }
    }
}