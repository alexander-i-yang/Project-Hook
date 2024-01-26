using System;
using ASK.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerArmManager : MonoBehaviour
    {
        [SerializeField] private GameObject leftUpper;
        [SerializeField] private GameObject leftLower;
        [SerializeField] private GameObject rightUpper;
        [SerializeField] private GameObject rightLower;

        private PlayerGrapplerStateMachine _input;
        private float _e;
        [SerializeField] private float duration;

        private void Awake()
        {
            _input = GetComponentInParent<PlayerGrapplerStateMachine>();
        }

        private void Update()
        {
            Quaternion i = new Quaternion();
            i.eulerAngles = new Vector3(0, 0, GetAngle());
            leftUpper.transform.rotation = i;
        }

        private float lastAngle = 0;
        float GetAngle()
        {
            
            float target = GetAngleRaw();
            if (transform.localScale.x < 0) target += 180;

            if (Mathf.Abs(target - lastAngle) > 180) target -= 180;
            
            float ret = Mathf.Lerp(lastAngle, target, duration);
            lastAngle = ret;
            return ret;
        }

        float GetAngleRaw()
        {
            float ret;
            if (_input.IsGrappling() || _input.IsGrappleExtending())
            {
                Vector2 aimPos = _input.CurGrapplePos() - (Vector2)transform.position;
                ret = Mathf.Atan2(aimPos.y, aimPos.x) * Mathf.Rad2Deg;
                return ret;
            }

            return 270;
        }
    }
}