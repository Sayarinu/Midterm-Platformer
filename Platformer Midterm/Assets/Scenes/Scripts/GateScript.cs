using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateScript : MonoBehaviour
{
    [SerializeField]
    int newBuildScene;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            other.gameObject.GetComponent<PlayerMove>().RemoveInput();
            FindObjectOfType<PauseMenu>().RemoveInput();
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            SceneManager.LoadScene(newBuildScene);
        }
    }
}
