using A2DK.Phys;
using UnityEngine;
using UnityEngine.Events;
using static Helpers.Helpers;

namespace Mechanics
{
    public class PullBehavior : MonoBehaviour, IGrappleable, IPullable

    {
        private Actor _myActor;

        private bool _inSticky;

        [SerializeField] private float minPullV;
        [SerializeField] private float initPullMag;
        [SerializeField] private float grappleLerp;
        [SerializeField] private float distanceScale;

        [SerializeField] private UnityEvent _onAttachGrapple;
        [SerializeField] private UnityEvent _onDetachGrapple;
        
        private void Awake()
        {
            _myActor = GetComponent<Actor>();
        }

        public (Vector2 curPoint, IGrappleable attachedTo, GrappleapleType grappleType) AttachGrapple(Actor p,
            Vector2 rayCastHit)
        {
            _onAttachGrapple?.Invoke();
            
            Vector2 apply = (p.transform.position - transform.position).normalized * initPullMag;

            Vector2 newV = CombineVectorsWithReset(p.velocity, apply);
            _myActor.SetVelocity(newV);
            return (transform.position, this, GrappleapleType.PULL);
        }

        public Vector2 ContinuousGrapplePos(Vector2 origPos, Actor grapplingActor)
        {
            Vector2 rawV = grapplingActor.transform.position - transform.position;

            if (_inSticky)
            {
                _myActor.StickyPullMove(rawV);
                return transform.position;
            }

            float newMag = rawV.magnitude * distanceScale;
            newMag = Mathf.Max(minPullV, newMag);

            Vector2 targetV = rawV.normalized * newMag;
            
            _myActor.ApplyVelocity(grappleLerp * targetV);
            
            _myActor.SetVelocity(Vector3.Project(_myActor.velocity, rawV));   
            
            // _myActor.SetVelocity(Vector2.Lerp(_myActor.velocity, newV, grappleLerp));

            return transform.position;

            /*var rb = GetComponent<Rigidbody2D>();
            rb.AddForceAtPosition(velocity, transform.position);
            return transform.position;*/
        }
        
        public void OnStickyEnter(Collider2D stickyCollider)
        {
            _inSticky = true;
        }

        public void OnStickyExit(Collider2D stickyCollider)
        {
            _inSticky = false;
        }

        public PhysObj GetPhysObj() => _myActor;

        public void DetachGrapple()
        {
            _onDetachGrapple?.Invoke();
        }
    }
}