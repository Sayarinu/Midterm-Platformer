using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    private PlayerInput input;

    private DefaultInputActions menuInput;

    public GameObject pauseMenuUI;

    private int select = 0;

    private void Start() {
        input = PublicVars.input;
        input.Player.pause.performed += PauseInput;
        input.UI.Cancel.performed += PauseInput;
    }

    public void PauseInput(InputAction.CallbackContext context) {
        print("pause");
        if (GameIsPaused) {
            Resume();
        } else {
            Pause();
        }
        
    }

    public void Resume(){
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        input.Player.Enable();
        input.UI.Disable();
    }
    void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        input.UI.Enable();
        input.Player.Disable();
    }

    public void LoadOptions() {

    }

    public void returnToTitle() {
        FindObjectOfType<PlayerMove>().RemoveInput();
        SceneManager.LoadScene("Title Screen");
    }

    public void RemoveInput(){
        input.Player.pause.performed -= PauseInput;
        input.UI.Cancel.performed -= PauseInput;
    }
}
