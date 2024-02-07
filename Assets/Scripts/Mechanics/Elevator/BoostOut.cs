using System.Collections;
using UnityEngine;

public class BoostOut : MonoBehaviour
{
    private float launchForce = 5f;
    private bool hasPlayerInput = false;
    private GameObject player;
    private Rigidbody2D playerRigidbody;

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
        yield return new WaitForSeconds(1f);
        // Pause the game
        Time.timeScale = 0f;

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
        Time.timeScale = 1f;

        // Launch the player to the mouse position
        LaunchPlayerToMousePosition();
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
