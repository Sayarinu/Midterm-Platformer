using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpoinScript : MonoBehaviour
{
    [SerializeField]
    Transform spawnPoint;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PublicVars.currentCheckpoint = spawnPoint.position;
        }
    }
}
