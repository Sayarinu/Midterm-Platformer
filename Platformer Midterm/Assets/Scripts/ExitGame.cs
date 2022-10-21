using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ExitGame : MonoBehaviour
{

    private PlayerInput input; 
    // Start is called before the first frame update
    void Start()
    {
        input = new PlayerInput();
        input.Player.Enable();
        input.Player.exit.performed += Exit;
        input.Player.die.performed += Reset;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exit(InputAction.CallbackContext context){
        Application.Quit();
    }

    public void Reset(InputAction.CallbackContext context){
        SceneManager.LoadScene("Title Screen");
    }
}
