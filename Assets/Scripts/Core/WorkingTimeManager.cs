using UnityEngine;
using System;
using ASK.Core;

namespace Core
{
    [RequireComponent(typeof(Timescaler))]
    public class WorkingTimeManager : TimeManager
    {
        private float _currentTimeScale = 1;
        [SerializeField] private float timeAcceleration = 4f;
        private float _previousTime = 0;

        public event Action<float> OnGetTimeScale;

        /*
		 * Smooths timescale transition between different states.
		 *
		 * @return currentTimeScale
		 */
        public override float GetTimeScale()
        {
            updateTimeScale();
            OnGetTimeScale?.Invoke(_currentTimeScale);
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
        }
    }
}