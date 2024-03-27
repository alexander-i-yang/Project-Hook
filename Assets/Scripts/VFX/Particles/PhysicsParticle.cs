using System.Collections;
using ASK.Helpers;
using UnityEngine;

namespace VFX
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PhysicsParticle : PrefabParticle
    {
        private Rigidbody2D _myRB;

        public override void Launch(Vector2 v, float rotationV)
        {
            _myRB = GetComponent<Rigidbody2D>();
            _myRB.AddForce(v, ForceMode2D.Impulse);
            _myRB.AddTorque(rotationV, ForceMode2D.Impulse);
        }

        void Update()
        {
            if (_myRB.velocity.magnitude > 0.1)
            {
                print(_myRB.velocity);
            }
        }
    }
}