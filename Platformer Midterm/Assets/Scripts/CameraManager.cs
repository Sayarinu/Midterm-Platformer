using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Camera _camera;
    bool currState;
    // Start is called before the first frame update
    void Start()
    {
        // _camera = this.GetComponent<Camera>();
        // _camera.aspect = 16 / 9;
        Screen.SetResolution(1920, 1080, PublicVars.fullscreen);
        currState = PublicVars.fullscreen;
    }

    private void Update() {
        if (currState != PublicVars.fullscreen) {
            Screen.SetResolution(1920, 1080, PublicVars.fullscreen);
            currState = PublicVars.fullscreen;
        }
    }
}
