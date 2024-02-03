using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject player;
    private GameObject currentElevator;
    private Elevator elevator;
    private BoostOut boostOutScript;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        boostOutScript = gameObject.AddComponent<BoostOut>();
    }

    void Update()
    {
        // Add logic here to check if the player has eliminated all entities!!!!!!!!!!!!!!
        if (currentElevator != null)
        {
            elevator = currentElevator.GetComponent<Elevator>();

            // Teleport the player to the elevator's destination
            player.transform.position = elevator.GetDestination();

            // Use boost mechanic as soon as you teleport to the other elevator
            boostOutScript.StartBoostOut();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "TeleportHitbox")
        {
            currentElevator = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.name == "TeleportHitbox")
        {
            if (other.gameObject == currentElevator)
            {
                currentElevator = null;
            }
        }
    }
}