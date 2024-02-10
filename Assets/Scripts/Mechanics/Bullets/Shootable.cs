using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is attached to any game object that can shoot bullets. 
/// Currently, it is set to just shoot at regular intervals; if we need to 
/// change this behavior, change the logic in the Update method.
/// 
/// If we want multiple different attack patterns, we may need to make this a base class with subclasses.
/// </summary>
public class Shootable : MonoBehaviour
{
    [SerializeField]
    public BulletPool bulletPool;
    
    [SerializeField]
    public float shootInterval = 1.0f;

    private float elapsedTime;

    void Start()
    {
        elapsedTime = 0f;
    }

    void Update()
    {
        // shoot at regular intervals
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= shootInterval)
        {
            Shoot();
            elapsedTime = 0f;
        }
    }

    void Shoot()
    {
        // get bullet from the pool
        GameObject bullet = bulletPool.GetBullet();

        // set bullet's position and direction
        bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);

        // activate bullet
        bullet.SetActive(true);
    }
}