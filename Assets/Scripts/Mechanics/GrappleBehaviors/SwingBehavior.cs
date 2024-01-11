using A2DK.Phys;
using ASK.Editor;
using UnityEngine;
using UnityEngine.Events;

namespace Mechanics
{
    public class SwingGrappleBehavior : MonoBehaviour, IGrappleable
    {
        private PhysObj _myPhysObj;
        [SerializeField] private UnityEvent onAttachGrapple;
        [SerializeField] private bool useAnchor;

        [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And,
            nameof(useAnchor))]
        [SerializeField]
        private Transform anchor;

        [ShowIf(ActionOnConditionFail.JustDisable, ConditionOperator.And,
            nameof(useAnchor))]
        [SerializeField]
        private Vector2 anchorOffset;
        
        void Awake()
        {
            _myPhysObj = GetComponent<PhysObj>();
        }

        public (Vector2, IGrappleable, GrappleapleType) AttachGrapple(Actor p, Vector2 rayCastHit)
        {
            onAttachGrapple?.Invoke();
            
            return (GetGrapplePos(rayCastHit), this, GrappleapleType.SWING);
        }
        public Vector2 ContinuousGrapplePos(Vector2 origPos, Actor grapplingActor) => GetGrapplePos(origPos);

        public Vector2 GetGrapplePos(Vector2 origPos)
        {
            if (useAnchor)
            {
                origPos = (Vector2)anchor.transform.position + anchorOffset;
            }

            return origPos;
        }
        
        public PhysObj GetPhysObj() => _myPhysObj;

        public void DetachGrapple() {}
    }
}