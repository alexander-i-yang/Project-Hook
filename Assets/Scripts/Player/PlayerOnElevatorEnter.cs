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
        public float launchMultiplier = 3f; // 3 seems to work very well in general but could be messed with
        public float timeToClick = 6f;

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
            
            // Start a coroutine to wait for player input
            StartCoroutine(WaitForPlayerInput());
        }

        private IEnumerator WaitForPlayerInput()
        {
            // Start the timer
            float timer = 0f;

            // Apply the time scale
            ts = Game.TimeManager.ApplyTimescale(timeScaleAmount, 2);

            // Continue looping until player input is received or the timer reaches 10 seconds
            while (!Input.GetMouseButtonDown(0) && timer < timeToClick)
            {
                // Increment the timer
                timer += Time.unscaledDeltaTime;

                yield return null; // Yield execution until the next frame
            }

            // If the timer reaches 10 seconds, skip the rest of the coroutine
            if (timer >= timeToClick)
            {
                Game.TimeManager.RemoveTimescale(ts);
                yield break;
            }

            // Player input received
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Cursor.lockState = CursorLockMode.None;
            Game.TimeManager.RemoveTimescale(ts);

            // Calculate the direction to the mouse position
            Vector2 launchDirection = (mousePosition - transform.position);

            // Call the Boost method on playerActor with launchDirection as parameter
            playerActor.Boost(launchDirection, launchMultiplier);
        }
    }
}