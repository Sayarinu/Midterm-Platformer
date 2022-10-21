using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimPowerup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            PublicVars.canSwim = true;
            Destroy(this.gameObject);
        }
    }
}
