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

        [SerializeField] private float gravity;
        [FormerlySerializedAs("_spacing")] [SerializeField] private float spacing;
        public void SetAttachedTo(Transform i) => _attachedTo = i;
        public void SetSpacing(float f) => spacing = f;

        private void Awake()
        {
            _lr = GetComponent<LineRenderer>();
        }

        private void Update()
        {
            _lr.SetPosition(0, transform.position);
            _lr.SetPosition(1, _attachedTo.position);
        }

        void FixedUpdate()
        {
            transform.position -= Vector3.down * gravity * Game.TimeManager.TimeScale;
            
            Vector2 targetPos = _attachedTo.position;
            Vector2 curPos = transform.position;

            // print((targetPos - curPos).magnitude);
            // if ((targetPos - curPos).magnitude < spacing + 0.1f) return;
            //
            // print(spacing + " " + (curPos - targetPos).normalized * spacing);
            // targetPos += (targetPos - curPos).normalized * spacing;
            //
            // transform.position = targetPos;

            Vector3 newPos = Vector3.Lerp(transform.position, targetPos, velocityDecay * Game.TimeManager.TimeScale);
            if (Vector3.Distance(newPos, _attachedTo.position) < spacing)
            {
                newPos = Vector3.MoveTowards(curPos, targetPos, Game.TimeManager.TimeScale*((curPos - targetPos).magnitude - spacing));
            }
            transform.position = newPos;
            print((_attachedTo.transform.position - transform.position).magnitude);
            // print(_attachedTo.position + " " + targetPos + " " + (_attachedTo.transform.position - transform.position).magnitude);
        }
    }
}