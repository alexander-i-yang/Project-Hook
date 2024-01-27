using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEntrance : Elevator
{
    private GameObject player;
    private GameObject[] elevatorDoors;

    public int openEnterDoorDistance = 25;
    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        elevatorDoors = GameObject.FindGameObjectsWithTag("EnterElevatorDoor");
    }

    /*void CountObjects()
    {
        // Find all objects of the specified type in the scene
        YourObjectType[] objectsOfType = FindObjectsOfType<YourObjectType>();

        return objectsOfType.Length;
    }*/

    public void Update()
    {
        int amt = 6;
        if (amt == 0){
            foreach (GameObject door in elevatorDoors)
            {
                if (Vector3.Distance (door.transform.position, player.transform.position) < openEnterDoorDistance)
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
}
