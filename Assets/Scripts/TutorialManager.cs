using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialCharacterLeft;  // Left character image
    public GameObject tutorialCharacterRight; // Right character image
    public Text speechBubbleText; // Text component for speech bubble messages
    public Button nextButton; // Button for progressing the tutorial
    public Button[] mainMenuButtons; // Array of main menu buttons to disable during tutorial
    public Button viewTutorialButton; // The button that allows players to replay the tutorial
    public GameObject speechBubbleBackground; // The background image for the speech text (speech bubble)

    private string[] tutorialMessages = {
        "Welcome to BrainDigits! I'll show you how to play.",
        "First, choose a game mode from the main menu.",
        "Each mode will challenge your math skills in a fun way!",
        "In challenge mode, you'll answer as many questions as possible.",
        "Good luck and have fun learning!"
    };

    private int currentMessageIndex = 0;

    void Start()
    {
        Debug.Log("Starting the tutorial system.");
        // TEMPORARILY reset PlayerPrefs for testing purposes
        //PlayerPrefs.DeleteAll(); // This will clear all saved PlayerPrefs values

        // Hide the "View Tutorial" button until the player has completed the tutorial
        if (viewTutorialButton != null)
        {
            viewTutorialButton.gameObject.SetActive(false);
        }

        // Check if the tutorial has been completed before
        if (PlayerPrefs.GetInt("hasCompletedTutorial", 0) == 1)
        {
            Debug.Log("Tutorial already completed.");
            EnableMainMenu(); // Skip tutorial and enable main menu
        }
        else
        {
            Debug.Log("Tutorial has not been completed. Starting tutorial.");
            ShowTutorial(); // Show tutorial if it hasn't been completed yet
        }
    }

    void ShowTutorial()
    {
        Debug.Log("Entering ShowTutorial()");

        // Disable main menu buttons during the tutorial
        foreach (Button button in mainMenuButtons)
        {
            button.interactable = false;
            Debug.Log($"{button.name} is now disabled.");
        }

        // Ensure the first character image is visible at the start
        tutorialCharacterLeft.SetActive(true);
        tutorialCharacterRight.SetActive(false);

        // Show the speech bubble background and text
        speechBubbleBackground.SetActive(true);
        speechBubbleText.gameObject.SetActive(true);

        // Set the initial message
        UpdateTutorial();
    }

    void UpdateTutorial()
    {
        if (speechBubbleText == null || tutorialCharacterLeft == null || tutorialCharacterRight == null)
        {
            Debug.LogError("One of the required components (speechBubbleText, tutorialCharacterLeft, tutorialCharacterRight) is not assigned in the Inspector.");
            return;
        }

        // Update the speech bubble message
        speechBubbleText.text = tutorialMessages[currentMessageIndex];

        // Show left image for the first three messages, then switch to the right image
        if (currentMessageIndex < 3)
        {
            tutorialCharacterLeft.SetActive(true);  // Show left image
            tutorialCharacterRight.SetActive(false); // Hide right image
        }
        else
        {
            tutorialCharacterLeft.SetActive(false); // Hide left image
            tutorialCharacterRight.SetActive(true);  // Show right image
        }

        Debug.Log($"Current message index: {currentMessageIndex}, Message: {tutorialMessages[currentMessageIndex]}");
    }

    public void OnNextButtonClicked()
    {
        Debug.Log($"Next button clicked, currentMessageIndex: {currentMessageIndex}");

        currentMessageIndex++;

        if (currentMessageIndex < tutorialMessages.Length)
        {
            UpdateTutorial();
        }
        else
        {
            CompleteTutorial();
        }
    }

    void CompleteTutorial()
    {
        Debug.Log("Entering CompleteTutorial()");

        // Mark the tutorial as completed
        PlayerPrefs.SetInt("hasCompletedTutorial", 1); // Only set PlayerPrefs here
        PlayerPrefs.Save();

        // Re-enable main menu buttons
        foreach (Button button in mainMenuButtons)
        {
            button.interactable = true;
            Debug.Log($"{button.name} is now enabled.");
        }

        // Hide tutorial UI elements
        tutorialCharacterLeft.SetActive(false);
        tutorialCharacterRight.SetActive(false);
        speechBubbleText.gameObject.SetActive(false);
        speechBubbleBackground.SetActive(false); // Hide the speech bubble background
        nextButton.gameObject.SetActive(false);

        // Show the "View Tutorial" button now that the tutorial is completed
        if (viewTutorialButton != null)
        {
            viewTutorialButton.gameObject.SetActive(true);
            Debug.Log("View Tutorial button is now active.");
        }
    }

    // Method to replay the tutorial
    public void ReplayTutorial()
    {
        // Reset the tutorial completion flag
        PlayerPrefs.SetInt("hasCompletedTutorial", 0); // Reset PlayerPrefs to show the tutorial again
        PlayerPrefs.Save();

        // Reload the current scene to restart the tutorial
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Helper method to enable the main menu if the tutorial has already been completed
    private void EnableMainMenu()
    {
        // Re-enable main menu buttons
        foreach (Button button in mainMenuButtons)
        {
            button.interactable = true;
        }

        // Show the "View Tutorial" button if the tutorial has been completed
        if (viewTutorialButton != null)
        {
            viewTutorialButton.gameObject.SetActive(true);
            Debug.Log("View Tutorial button is visible.");
        }

        // Hide the tutorial elements since it's completed
        tutorialCharacterLeft.SetActive(false);
        tutorialCharacterRight.SetActive(false);
        speechBubbleText.gameObject.SetActive(false);
        speechBubbleBackground.SetActive(false); // Hide the speech bubble background
        nextButton.gameObject.SetActive(false);
    }
}
