using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SectionManager : MonoBehaviour
{
    [SerializeField]
    GameObject VCamObj;
    CinemachineVirtualCamera VCam;

    private void Start() {
        VCam = VCamObj.GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            // when the player enters the screen
            VCam.Priority = 10;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            // when the player exits the screen
            VCam.Priority = -10;
        }
    }
}
