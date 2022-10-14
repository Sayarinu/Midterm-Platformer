using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEnemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.tag == "Respawn") {
            for (int i = 0; i < other.transform.childCount; i++) {
                other.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.transform.tag == "Respawn") {
            for (int i = 0; i < other.transform.childCount; i++) {
                other.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
}
