using System;
using ASK.Core;
using UnityEngine;

namespace VFX
{
    [RequireComponent(typeof(TrailRenderer))]
    public class TrailManager : MonoBehaviour
    {
        private TrailRenderer _tr;
        private float _rawTime;

        private float _yee;
        public float Yee => _yee;

        private void Awake()
        {
            _tr = GetComponent<TrailRenderer>();
            _rawTime = _tr.time;
        }

        private void OnEnable()
        {
            Game.TimeManager.OnTimeScaleChange += ChangeTimeScale;
        }

        private void OnDisable()
        {
            Game.TimeManager.OnTimeScaleChange -= ChangeTimeScale;
        }

        private void ChangeTimeScale()
        {
            _tr.time = _rawTime / Game.TimeManager.TimeScale;
        }
    }
}