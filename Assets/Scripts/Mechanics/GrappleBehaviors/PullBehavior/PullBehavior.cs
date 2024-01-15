using A2DK.Phys;
using UnityEngine;
using UnityEngine.Events;
using static Helpers.Helpers;

namespace Mechanics
{
    [RequireComponent(typeof(PullBehaviorStateMachine), typeof(Actor))]
    public class PullBehavior : MonoBehaviour, IGrappleable, IPullable

    {
        private Actor _myActor;
        private PullBehaviorStateMachine _sm;

        [SerializeField] private float minPullV;
        public float MinPullV => minPullV;
        [SerializeField] private float initPullMag;
        public float InitPullMag => initPullMag;
        [SerializeField] private float grappleLerp;
        public float GrappleLerp => grappleLerp;
        [SerializeField] private float distanceScale;
        public float DistanceScale => distanceScale;
        [SerializeField] private float keepVGraceTime;
        public float KeepVGraceTime => keepVGraceTime;

        [SerializeField] private UnityEvent _onAttachGrapple;
        [SerializeField] private UnityEvent _onDetachGrapple;
        
        private void Awake()
        {
            _myActor = GetComponent<Actor>();
            _sm = GetComponent<PullBehaviorStateMachine>();
        }

        public (Vector2 curPoint, IGrappleable attachedTo, GrappleapleType grappleType) AttachGrapple(Actor p,
            Vector2 rayCastHit)
        {
            _sm.CurrState.AttachGrapple();
            _onAttachGrapple?.Invoke();
            
            Vector2 apply = (p.transform.position - transform.position).normalized * initPullMag;

            Vector2 newV = CombineVectorsWithReset(p.velocity, apply);
            _myActor.SetVelocity(newV);
            
            return (transform.position, this, GrappleapleType.PULL);
        }

        public Vector2 ContinuousGrapplePos(Vector2 grapplePos, Actor grapplingActor)
        {
            Vector2 rawV = grapplingActor.transform.position - transform.position;
            _sm.CurrState.ContinuousGrapplePos(rawV, _myActor);
            return transform.position;
        }
        
        public void OnStickyEnter(Collider2D stickyCollider)
        {
            _sm.CurrState.StickyEnter(_myActor.velocity, stickyCollider.transform);
        }

        public void OnStickyExit(Collider2D stickyCollider)
        {
            _sm.CurrState.StickyExit();
        }

        public PhysObj GetPhysObj() => _myActor;

        public void DetachGrapple()
        {
            _sm.CurrState.DetachGrapple();
            _onDetachGrapple?.Invoke();
        }

        public void SetV(Vector2 inputBeforeStickyV)
        {
            _myActor.SetVelocity(inputBeforeStickyV);
        }
    }
}