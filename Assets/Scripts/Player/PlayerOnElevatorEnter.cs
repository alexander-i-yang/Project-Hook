using System.Collections;
using Mechanics;
using UnityEngine;
using ASK.Helpers;
using A2DK.Phys;
using ASK.Core;

namespace Player
{
    public class PlayerOnElevatorEnter : OnElevatorEnter
    {
        [SerializeField] private float delay;
        private PlayerActor playerActor; // Reference to PlayerActor component
        private bool hasPlayerInput = false;
        private Timescaler.TimeScale ts;
        public float timeScaleAmount = 0.1f;

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
            ts = Game.TimeManager.ApplyTimescale(timeScaleAmount, 3);
            
            // Start a coroutine to wait for player input
            StartCoroutine(WaitForPlayerInput());
        }

        private IEnumerator WaitForPlayerInput()
        {
            // Continue looping until player input is received
            while (!Input.GetMouseButtonDown(0))
            {
                yield return null; // Yield execution until the next frame
            }

            // Player input received
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Game.TimeManager.RemoveTimescale(ts);
            Debug.Log("Got mouse position");

            // Calculate the direction to the mouse position
            Vector2 launchDirection = (mousePosition - transform.position);

            // Call the Boost method on playerActor with launchDirection as parameter
            playerActor.Boost(launchDirection);
        }
    }
}