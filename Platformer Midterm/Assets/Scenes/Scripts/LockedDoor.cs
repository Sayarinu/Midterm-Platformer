using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField]
    GameObject unlockedDoor;
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")) {
            if (PublicVars.numberOfKeys > 0) {
                PublicVars.numberOfKeys -= 1;
                Instantiate(
                    unlockedDoor,
                    this.transform.position,
                    Quaternion.identity
                );
                Destroy(this.gameObject);
            }
        }
    }
}
