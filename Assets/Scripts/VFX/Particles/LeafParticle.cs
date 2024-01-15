using System;
using ASK.Core;
using MyBox;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VFX
{
    public class LeafParticle : PrefabParticle
    {
        [MinMaxRange(0, 5), SerializeField] private RangedFloat gravity = new (0.1f, 0.5f);
        private float _gravity;
        [MinMaxRange(0, 50), SerializeField] private RangedFloat arcDistance = new (10, 20);
        private float _arcDistance;
        
        [MinMaxRange(0, 10), SerializeField] private RangedFloat waveSpeed = new (1, 2);
        private float _waveSpeed;
        [MinMaxRange(0, 2), SerializeField] private RangedFloat arcAngle = new (0.75f, 1.25f);
        private float _arcAngle;

        private Vector3 _anchor = Vector3.zero;

        private Vector2 _velocity;
        [SerializeField] private float _minFloatV;
        [SerializeField] private float _drag;

        private bool _isFloating;

        [SerializeField] private float _gravityFly;

        void Awake()
        {
            _anchor = transform.position;
            RecalcParams();
        }

        void RecalcParams()
        {
            _gravity = Random.Range(gravity.Min, gravity.Max);
            _arcDistance = Random.Range(arcDistance.Min, arcDistance.Max);
            _waveSpeed = Random.Range(waveSpeed.Min, waveSpeed.Max);
            _arcAngle = Random.Range(arcAngle.Min, arcAngle.Max);
        }
        
        void Update()
        {
            if (Mathf.Abs(_velocity.y) < _minFloatV && !_isFloating)
            {
                _anchor = transform.position + Vector3.up * _arcDistance;
                _isFloating = true;
            }

            if (_isFloating)
            {
                FloatUpdate();
            }
            else
            {
                FlyUpdate();
            }
        }

        void FlyUpdate()
        {
            transform.position += (Vector3) _velocity * Game.TimeManager.TimeScale;
            var vSigns = new Vector2(Mathf.Sign(_velocity.x), Mathf.Sign(_velocity.y));
            _velocity -= vSigns * (_drag / Game.TimeManager.TimeScale);
            _velocity += Vector2.down * (_gravityFly * Game.TimeManager.TimeScale);
        }
        
        void FloatUpdate()
        {
            float angle = Mathf.Sin(Game.TimeManager.Time * _waveSpeed)*(_arcAngle) - Mathf.PI/2;

            transform.position = _anchor + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * _arcDistance;
            _anchor += Vector3.down * (_gravity * Game.TimeManager.TimeScale);
        }

        public override void Launch(Vector2 v, float rotationV)
        {
            _velocity = v;
        }
    }
}