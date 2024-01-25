using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorExit : Elevator
{
    private GameObject player;
    private GameObject[] elevatorDoors;

    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        elevatorDoors = GameObject.FindGameObjectsWithTag("ElevatorDoor");
    }

    public void Update()
    {
        foreach (GameObject door in elevatorDoors)
        {
            if (Vector3.Distance (door.transform.position, player.transform.position) < 20)
            {
                door.SetActive(false);
            }
            else
            {
                door.SetActive(true);
            }
        }
    }
}
