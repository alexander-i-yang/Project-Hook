using System.Collections;
using System.Collections.Generic;
using ASK.UI;
using Player;
using UnityEngine;

/**
 * Gives input from playerCore to the PauseUIController.
 */
[RequireComponent(typeof(PauseUIController))]
public class PauseInputProvider : MonoBehaviour
{
    private PauseUIController _pauseUI;
    void Awake()
    {
        _pauseUI = GetComponent<PauseUIController>();
    }
    
    void OnEnable()
    {
        FindObjectOfType<PlayerInputController>()?.AddToPauseAction(OnPausePressed);
    }
    
    void OnDisable()
    {
        FindObjectOfType<PlayerInputController>()?.AddToPauseAction(OnPausePressed);
    }

    void OnPausePressed()
    {
        _pauseUI.OnPausePressed();
    }
}
