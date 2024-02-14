using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is attached to the SceneEssentials game object in the scene.
/// It is used to manage the bullet pool.
/// </summary>
public class BulletPool : MonoBehaviour
{
    private Transform _child;

    [SerializeField]
    GameObject bulletPrefab;

    // initial pool size - will increase during runtime as needed
    [SerializeField]
    private int poolSize = 0;

    private List<GameObject> bullets;

    void Awake()
    {
        // create a child object to hold the bullets
        _child = new GameObject("Bullet Pool").transform;
        _child.parent = transform;
    }

    void Start()
    {
        InitializeBulletPool();
    }

    void InitializeBulletPool()
    {
        bullets = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, _child);
            bullet.SetActive(false);
            bullets.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach (GameObject bullet in bullets)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }
        
        // if no inactive bullets, create a new one
        GameObject newBullet = Instantiate(bulletPrefab, _child);
        bullets.Add(newBullet);
        return newBullet;
    }
}
