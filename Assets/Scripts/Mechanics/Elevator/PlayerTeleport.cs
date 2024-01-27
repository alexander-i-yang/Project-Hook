using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currentElevator;
    private bool isInElevator = false;
    public float teleportCooldown = 2f; // Set the cooldown time in seconds
    private float teleportTimer = 0f;
    private bool canTeleport = true;
    // Update is called once per frame
    void Update()
    {
        // Check if the teleport cooldown has elapsed
        if (teleportTimer <= 0f && canTeleport && currentElevator != null)
        {
            transform.position = currentElevator.GetComponent<Elevator>().GetDestination().position;
            canTeleport = false;
            teleportTimer = teleportCooldown; // Reset the cooldown timer
        }

        // Update the teleport timer
        teleportTimer -= Time.deltaTime;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Teleport")){
            currentElevator = other.gameObject;
            if (teleportTimer <= 0f)
            {
                canTeleport = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Teleport")){
            if (other.gameObject == currentElevator){
                currentElevator = null;
                isInElevator = false;
            }
        }
    }
}
