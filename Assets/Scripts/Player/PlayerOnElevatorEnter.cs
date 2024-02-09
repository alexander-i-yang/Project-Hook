using System.Collections;
using Mechanics;
using UnityEngine;
using ASK.Helpers;

namespace Player
{
    public class PlayerOnElevatorEnter : OnElevatorEnter
    {
        [SerializeField] private float delay;
        private PlayerActor playerActor; // Reference to PlayerActor component

        private void Start()
        {
            // Get reference to PlayerActor component
            playerActor = GetComponent<PlayerActor>();
        }

        public override void OnEnter(Elevator elevator)
        {
            StartCoroutine(Helper.DelayAction(delay, () => Teleport(elevator)));
        }

        private void Teleport(Elevator elevator)
        {
            transform.position = elevator.GetDestination().transform.position;
            StartCoroutine(BoostMechanic());
        }
        IEnumerator BoostMechanic()
        {
            yield return new WaitForSeconds(7);
            playerActor.Boost();
        }
    }
}