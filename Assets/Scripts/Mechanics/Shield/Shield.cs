using A2DK.Phys;
using Combat;
using Helpers;
using UnityEngine;

namespace Mechanics
{
    [RequireComponent(typeof(PullBehavior))]
    public class Shield : Crate, IPunchable
    {
        private PullBehavior _pullBehavior;

        [SerializeField] private int bounceTimes = 3;
        private int _bouncesLeft;

        void Awake()
        {
            _pullBehavior = GetComponent<PullBehavior>();
            base.Awake();
        }

        public override bool OnCollide(PhysObj p, Vector2 direction)
        {
            Vector2 v = velocity;
            bool ret = base.OnCollide(p, direction);
            if (ret && _bouncesLeft > 0)
            {
                v.Scale(direction * -100);
                velocity = v;
                _bouncesLeft--;
            }

            return ret;
        }

        public new bool ReceivePunch(Vector2 v)
        {
            if (_pullBehavior.IsInSticky)
            {
                _bouncesLeft = bounceTimes;
            }

            return base.ReceivePunch(v);
        }
    }
}