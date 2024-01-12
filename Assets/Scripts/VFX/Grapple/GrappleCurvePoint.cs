using System;
using UnityEngine;

namespace VFX
{
    public class GrappleCurvePoint : MonoBehaviour
    {
        [NonSerialized] public float Angle;

        public void CalcPos(float speed, float attachedAngle, Vector2 origin, float magnitude)
        {
            Angle = Mathf.LerpUnclamped(Angle, attachedAngle, speed);
            Vector2 direction = new Vector2(Mathf.Cos(Angle * Mathf.Deg2Rad), Mathf.Sin(Angle * Mathf.Deg2Rad));
            transform.position = origin + direction * magnitude;
        }
    }
}