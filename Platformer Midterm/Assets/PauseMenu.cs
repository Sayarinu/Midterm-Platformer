using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    private PlayerInput input;

    public GameObject pauseMenuUI;

    private void Start() {
        input = PublicVars.input;
        input.UI.pause.performed += PauseInput;
        input.Player.pause.performed += PauseInput;
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
        SceneManager.LoadScene("Title Screen");
    }
}
