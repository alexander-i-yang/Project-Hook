using System.Collections;
using Mechanics;
using UnityEngine;
using ASK.Helpers;

namespace Player
{
    public class PlayerOnElevatorEnter : OnElevatorEnter
    {
        [SerializeField] private float delay;
        
        public override void OnEnter(Elevator elevator)
        {
            StartCoroutine(Helper.DelayAction(delay, () => Teleport(elevator)));
        }

        private void Teleport(Elevator elevator)
        {
            transform.position = elevator.GetDestination().transform.position;
        }
    }
}