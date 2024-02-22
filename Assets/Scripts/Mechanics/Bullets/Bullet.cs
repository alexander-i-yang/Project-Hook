using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UIElements;
using ASK.Core;

public class Bullet : MonoBehaviour
{
    // bullet position
    [SerializeField]
    Vector3 position;

    // bullet speed
    [SerializeField]
    float speed = 5f;


    // time before bullet is deactivated
    [SerializeField]
    float lifeTime = 10f;
    float elapsedTime = 0f;

    private void OnEnable()
    {
        elapsedTime = 0f; // reset lifetime
    }

    void Update()
    {
        MoveBullet();

        // bullet lifetime
        elapsedTime += Game.TimeManager.GetTimeScale();
        if (elapsedTime >= lifeTime)
        {
            gameObject.SetActive(false);
        }
    }

    void MoveBullet()
    {
        // move bullet
        position = transform.position;
        position += speed * Game.TimeManager.GetTimeScale() * transform.right; 
        transform.position = position;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        targetable temp = other.GetComponent<targetable>();
        if (temp != null)
        {
            // debugging
            Debug.Log("Bullet collided with " + temp.GetType());
            if (temp.GetType() == objectType.Player)
            {
                print("Player collide");
            }

            gameObject.SetActive(false);
        }
        
    }
    

    // deactivate after a collision
        
}

