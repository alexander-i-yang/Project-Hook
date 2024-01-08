using System;
using A2DK.Phys;
using ASK.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace VFX
{
    [RequireComponent(typeof(LineRenderer))]
    public class ScarfPoint : MonoBehaviour
    {
        [SerializeField] private float velocityDecay;
        [SerializeField] private Transform _attachedTo;

        private LineRenderer _lr;
        private Scarf _scarfManager;

        [SerializeField] private float gravity;
        public void SetAttachedTo(Transform i) => _attachedTo = i;

        private void Awake()
        {
            _lr = GetComponent<LineRenderer>();
            _scarfManager = GetComponentInParent<Scarf>();
        }

        private void Update()
        {
            _lr.SetPosition(0, transform.position);
            _lr.SetPosition(1, _attachedTo.position);
        }

        public void CalcPos(float minSpacing, float maxSpacing)
        {
                transform.position -= Vector3.down * gravity * Game.TimeManager.TimeScale;
            
            Vector2 targetPos = _attachedTo.position;
            Vector2 curPos = transform.position;
            
            Vector3 newPos = Vector3.Lerp(transform.position, targetPos, velocityDecay * Game.TimeManager.TimeScale);
            if (Vector3.Distance(newPos, _attachedTo.position) < minSpacing)
            {
                newPos = Vector3.MoveTowards(curPos, targetPos, Game.TimeManager.TimeScale*((curPos - targetPos).magnitude - minSpacing));
            }
            transform.position = newPos;
        }
    }
}