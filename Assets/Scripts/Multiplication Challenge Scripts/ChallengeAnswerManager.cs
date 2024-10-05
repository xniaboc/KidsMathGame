using UnityEngine;
using UnityEngine.UI;

public class ChallengeAnswerManager : MonoBehaviour
{
    public Button[] answerButtons;       // Assign the answer buttons in the Inspector (ensure 4 buttons)
    private int correctAnswer;           // Store the correct answer for the current question
    private MultiplicationChallengeManager challengeManager;  // Reference to the ChallengeManager

    void Start()
    {
        // Find the MultiplicationChallengeManager in the scene
        challengeManager = FindObjectOfType<MultiplicationChallengeManager>();

        if (challengeManager == null)
        {
            Debug.LogError("MultiplicationChallengeManager not found in the scene!");
        }
    }

    // Method to assign answers to the buttons
    public void AssignAnswers(int[] answers, int correct)
    {
        correctAnswer = correct;

        // Debugging: Log how many answers and buttons we have
        Debug.Log("Assigning " + answers.Length + " answers to " + answerButtons.Length + " buttons");

        // Check if the number of answers matches the number of buttons
        if (answers.Length != answerButtons.Length)
        {
            Debug.LogError("Mismatch between number of answers (" + answers.Length + ") and number of answer buttons (" + answerButtons.Length + ")");
            return;
        }

        // Assign each answer to the corresponding button
        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = answers[i]; // Capture the answer for closure
            Button button = answerButtons[i]; // Capture the button for closure
            button.GetComponentInChildren<Text>().text = answer.ToString();  // Set button text to answer value
            button.onClick.RemoveAllListeners();                             // Remove old listeners

            // Use proper closure for button listeners
            button.onClick.AddListener(() => OnAnswerSelected(button, answer));
        }
    }

    // Method called when an answer is selected
    public void OnAnswerSelected(Button selectedButton, int selectedAnswer)
    {
        // Check if the selected answer is correct
        bool isCorrect = selectedAnswer == correctAnswer;

        // Call OnQuestionAnswered in the MultiplicationChallengeManager
        if (challengeManager != null)
        {
            challengeManager.OnQuestionAnswered(isCorrect, selectedButton);
        }
        else
        {
            Debug.LogError("MultiplicationChallengeManager not assigned or not found!");
        }
    }

    // Method to get answer buttons for highlighting
    public Button[] GetAnswerButtons()
    {
        return answerButtons;
    }
}
