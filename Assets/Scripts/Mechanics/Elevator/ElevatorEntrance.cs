using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorEntrance : Elevator
{
    private GameObject player;
    [SerializeField] private GameObject elevatorDoors;

    public void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject == player){
            elevatorDoors.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject == player){
            elevatorDoors.SetActive(true);
        }
    }
}
