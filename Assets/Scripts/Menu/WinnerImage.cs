using UnityEngine;
using UnityEngine.SceneManagement;

public class WinnerImage : MonoBehaviour
{
    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0))
        {
            // Load the main menu scene
            SceneManager.LoadScene("MenuScene");
        }
    }
}
