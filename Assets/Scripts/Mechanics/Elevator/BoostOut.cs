using System.Collections;
using UnityEngine;
using ASK.Core;
using Helpers;
using Mechanics;
using A2DK.Phys;


public class BoostOut : MonoBehaviour
{
    private bool hasPlayerInput = false;
    private GameObject player;
    private Rigidbody2D playerRigidbody;
    private Timescaler.TimeScale ts;


    void Start()
    {
        // You may want to get the Rigidbody2D component in Start if it's used later
        player = GameObject.FindGameObjectWithTag("Player");
        playerRigidbody = player.GetComponent<Rigidbody2D>();
    }

    public void StartBoostOut()
    {
        StartCoroutine(WaitForPlayerInput());
    }

    IEnumerator WaitForPlayerInput()
    {
        // Pause the game
        ts = Game.TimeManager.ApplyTimescale(0,3);

        // Continue looping until player input is received
        while (!hasPlayerInput)
        {
            // Check for player input (for example, pressing the mouse button)
            if (Input.GetMouseButtonDown(0))
            {
                // Player has inputted the action
                hasPlayerInput = true;
            }

            // Yield execution until the next frame
            yield return null;
        }

        // Unpause the game
        Game.TimeManager.RemoveTimescale(ts);

        // Launch the player to the mouse position
        // boost goes HERE
    }

    /*void LaunchPlayerToMousePosition()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Calculate the direction to the mouse position
        Vector2 launchDirection = (mousePosition - transform.position).normalized;

        // Apply a force to launch the player
        playerRigidbody.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
    }*/
}
