using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostOut : MonoBehaviour
{
    private float launchForce = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        StartCoroutine(WaitForPlayerInput());
        Time.timeScale = 1f;
        MovePlayerToMousePosition();
    }

    IEnumerator WaitForPlayerInput()
    {
        // Continue looping until player input is received
        while (!hasPlayerInput)
        {
            // Check for player input (for example, pressing the space key)
            if (Input.GetMouseButtonDown(0))
            {
                // Player has inputted the action
                hasPlayerInput = true;
            }

            // Yield execution until the next frame
            yield return null;
        }
    }

    void LaunchPlayerToMousePosition()
    {
        // Get the mouse position in world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // Calculate the direction to the mouse position
        Vector2 launchDirection = (mousePosition - transform.position).normalized;

        // Apply a force to launch the player
        playerRigidbody.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);
    }
}
