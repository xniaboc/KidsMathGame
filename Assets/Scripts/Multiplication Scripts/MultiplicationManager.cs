using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplicationManager : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}
