using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorExit : Elevator
{
    private GameObject player;
    private GameObject[] elevatorDoors;

    public int openExitDoorDistance = 25;

    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        elevatorDoors = GameObject.FindGameObjectsWithTag("ExitElevatorDoor");
    }

    public void Update()
    {
        foreach (GameObject door in elevatorDoors)
        {
            if (Vector3.Distance (door.transform.position, player.transform.position) < openExitDoorDistance)
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
