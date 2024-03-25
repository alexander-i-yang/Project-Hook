using UnityEngine;
using System;
using ASK.Core;

namespace Core
{
    public class AnimationTimescale : MonoBehaviour
    {
        private Animator _animationController;

        private void Start()
        {
            _animationController = gameObject.GetComponent<Animator>();
            //I am not sure if this is a good way to get the WorkingTimeManager & allow this to subscribe to the event
            WorkingTimeManager workingTimeManager = FindObjectsOfType<WorkingTimeManager>()[0];
            workingTimeManager.OnGetTimeScale += UpdateAnimationSpeed;
        }

        private void UpdateAnimationSpeed(float speed)
        {
            _animationController.speed = speed;
        }
    }
}
