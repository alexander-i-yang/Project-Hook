using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Player{
    public class IntroCutscene : MonoBehaviour
    {
        private PlayerInputController playerInputController;
        public PlayableDirector cutsceneTimeline1;
        public GameObject grab;
        public GameObject punch;
        private bool checking = false;
        public GameObject playerObject;
        public GameObject otherObject;
        public float detectionRange = 1f;

        private void OnTriggerEnter2D(Collider2D other) {
            if(other.tag == "Player")
            {
                Debug.Log("COLLIDE");

                // Play the cutscene Timeline
                cutsceneTimeline1.Play();
                playerInputController.OnDisable();

                // Disable the trigger collider to prevent re-triggering
                Collider2D collider = GetComponent<Collider2D>();
                collider.enabled = false;
            }
        }

        public void Start()
        {
            GameObject playerGameObject = GameObject.FindWithTag("Player");
            playerInputController = playerGameObject.GetComponent<PlayerInputController>();
        }

        public void secondPart()
        {
            cutsceneTimeline1.Pause();
            playerInputController.OnEnable();
            grab.SetActive(true);
            checking = true;
        }
        
        public void Update(){
            if (checking) {
                float distance = Vector3.Distance(playerObject.transform.position, otherObject.transform.position);
                if (distance <= detectionRange) {
                    grab.SetActive(false);
                    punch.SetActive(true);
                    cutsceneTimeline1.Play();
                }
            }
        }
    }
}

