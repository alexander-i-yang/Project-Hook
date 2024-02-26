using UnityEngine;
using System;
using ASK.Core;

namespace Core
{
    [RequireComponent(typeof(Timescaler))]
    public class WorkingTimeManager : TimeManager
    {
        private float _currentTimeScale = 1;
        [SerializeField] private float timeAcceleration = 16f;
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
            if (_currentTimeScale < 0)
            {
                Debug.Log("Negative timescale; this shouldn't happen. The TimeScale was reset to 1 to prevent further erroring. Please let Carter/PieBob know any details to potentially fix!");
                _currentTimeScale = 1;
                return;
            }

            float timeDifference = UnityEngine.Time.time - _previousTime;
            _previousTime = UnityEngine.Time.time;

            float targetTimeScale = base.GetTimeScale();
            Debug.Log("Current timescale: " + _currentTimeScale + "\nTarget timescale: " + targetTimeScale);
            float scaleFactor = (float) Math.Pow(timeAcceleration, timeDifference) - 1;
            _currentTimeScale = Mathf.MoveTowards(_currentTimeScale, targetTimeScale, _currentTimeScale * scaleFactor);
        }
    }
}