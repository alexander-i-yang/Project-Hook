using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currentElevator;

    // Update is called once per frame
    void Update()
    {
        // replace with logic to wait until all entities are destroyed

        if (currentElevator != null){
            transform.position = currentElevator.GetComponent<Elevator>().GetDestination().position;
        }

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Elevator")){
            currentElevator = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Elevator")){
            if (other.gameObject == currentElevator){
                currentElevator = null;
            }
        }
    }
}
