using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class VCamController : MonoBehaviour
{
    [SerializeField]
    GameObject VCamObj;
    CinemachineVirtualCamera VCam;

    private void Start() {
        VCam = VCamObj.GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            VCam.Priority = 10;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            VCam.Priority = -10;
        }
    }
}
