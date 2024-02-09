using UnityEngine;
using System;
using ASK.Core;

namespace Core
{
    [RequireComponent(typeof(Timescaler))]
    public class WorkingTimeManager : TimeManager
    {
        private float _currentTimeScale = 1;
        [SerializeField] private float timeAcceleration = 0.45f;
        private Animator _animator;
        private float _previousTime;


        //private void Awake()
        //{
        //    _animator = GetComponent<Animator>();
        //    _previousTime = UnityEngine.Time.time;
        //}

        /*
		 * Smooths timescale transition between different states.
		 *
		 * @return currentTimeScale
		 */
        public override float GetTimeScale()
        {
            updateTimeScale();

            return _currentTimeScale;
        }

        /*
		 * Updates the timeScale in a smooth manner using the time since the last update and timeAcceleration
		 *
		 */
        private void updateTimeScale()
        {
            float timeDifference = UnityEngine.Time.time - _previousTime;
            _previousTime = UnityEngine.Time.time;

            float targetTimeScale = base.GetTimeScale();
            float scaleFactor = timeAcceleration * timeDifference;
            _currentTimeScale = Mathf.MoveTowards(_currentTimeScale, targetTimeScale, _currentTimeScale * scaleFactor);
            _animator.speed = _currentTimeScale;
        }
    }
}