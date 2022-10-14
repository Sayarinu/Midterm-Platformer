using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEnemy : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy")
            other.gameObject.SetActive(true);
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Enemy")
            other.gameObject.SetActive(false);
    }
}
