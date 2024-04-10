using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IntroCutscene : MonoBehaviour
{
    public PlayableDirector cutsceneTimeline1;

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("COLLIDE");

            // Play the cutscene Timeline
            cutsceneTimeline1.Play();

            // Disable the trigger collider to prevent re-triggering
            GetComponent<Collider>().enabled = false;
        }
    }

    public void Update()
    {

    }
}

