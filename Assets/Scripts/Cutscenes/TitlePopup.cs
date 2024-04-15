using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitlePopup : MonoBehaviour
{
    public GameObject title;
    private float countdownTimer = 8f;
    private bool timer = false;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            Debug.Log("COLLIDE");

            // Logo Pops up
            title.SetActive(true);
            timer = true;
            GetComponent<Collider>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timer)
        {
            countdownTimer -= Time.deltaTime;
            if (countdownTimer <= 0)
            {
                title.SetActive(false);
                timer = false;
            }
        }
    }
}
