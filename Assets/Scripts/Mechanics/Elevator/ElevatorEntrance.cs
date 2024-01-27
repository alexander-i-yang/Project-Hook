using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEntrance : Elevator
{
    private GameObject player;
    private GameObject[] elevatorDoors;

    public int openDoorDistance = 25;
    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        elevatorDoors = GameObject.FindGameObjectsWithTag("ElevatorDoor");
    }

    /*void CountObjects()
    {
        // Find all objects of the specified type in the scene
        YourObjectType[] objectsOfType = FindObjectsOfType<YourObjectType>();

        return objectsOfType.Length;
    }*/

    public void Update()
    {
        //int amount = Invoke("CountObjects");
        //if (amount == 0){
            foreach (GameObject door in elevatorDoors)
            {
                if (Vector3.Distance (door.transform.position, player.transform.position) < openDoorDistance)
                {
                    door.SetActive(false);
                }
                else
                {
                    door.SetActive(true);
                }
            }
        //}
    }
}
