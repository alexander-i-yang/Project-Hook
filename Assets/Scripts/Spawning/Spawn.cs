using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Spawning
{
    public class Spawn : MonoBehaviour
    {
        private PlayerSpawnManager _player;

        private void OnTriggerEnter2D(Collider2D col)
        {
            PlayerSpawnManager spawnManager = col.GetComponent<PlayerSpawnManager>();
            if (spawnManager != null)
            {
                spawnManager.CurrentSpawnPoint = this;
            }
        }
    
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(10, 24, 1));
        }
#endif
    }
}