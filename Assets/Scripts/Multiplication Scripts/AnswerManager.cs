using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnswerManager : MonoBehaviour
{
    public Button[] answerButtons; // Assign the 4 buttons in the Inspector
    private int correctAnswer;
    private int questionCount = 0; // Track the number of questions asked
    private const int maxQuestions = 5; // Maximum questions
    private int[] answers; // Store the answers
    private NumberManager numberManager; // Reference to NumberManager to show correct answer prefabs

    void Start()
    {
        // Find the NumberManager in the scene
        numberManager = FindObjectOfType<NumberManager>();
        if (numberManager == null)
        {
            Debug.LogError("NumberManager not found in the scene!");
        }
    }

    public void AssignAnswers(int[] answersArray, int correctAns)
    {
        answers = answersArray; // Store the answers
        correctAnswer = correctAns;
        questionCount++;

        // Reset button colors and enable buttons before presenting the new question
        ResetButtonColorsAndEnable();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = answers[i];
            answerButtons[i].GetComponentInChildren<Text>().text = answer.ToString();

            // Clear previous listeners
            answerButtons[i].onClick.RemoveAllListeners();
            int index = i; // Capture current index for the listener
            answerButtons[i].onClick.AddListener(() => CheckAnswer(answer, index)); // Add listener
        }
    }

    private void CheckAnswer(int selectedAnswer, int buttonIndex)
    {
        Debug.Log("Button Clicked: " + selectedAnswer + " at index: " + buttonIndex);

        // Disable all buttons to prevent multiple clicks
        DisableAllButtons();

        // Check if the selected answer is correct
        if (selectedAnswer == correctAnswer)
        {
            // Highlight the clicked button green for correct
            SetButtonColor(answerButtons[buttonIndex], Color.green);
            Debug.Log("Correct Answer Selected");
        }
        else
        {
            // Highlight the clicked button red for incorrect
            SetButtonColor(answerButtons[buttonIndex], Color.red);
            Debug.Log("Incorrect Answer Selected");

            // Highlight the correct answer green
            HighlightCorrectAnswer();
        }

        // Show the correct answer prefabs, whether the answer is correct or wrong
        if (numberManager != null)
        {
            numberManager.ShowCorrectAnswer(correctAnswer);
        }

        // Start coroutine to handle the question transition
        StartCoroutine(ShowCorrectAnswerAndNextQuestion());
    }

    private void HighlightCorrectAnswer()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (answers[i] == correctAnswer)
            {
                // Highlight the correct answer button green
                SetButtonColor(answerButtons[i], Color.green);
                Debug.Log("Highlighting Correct Answer Button");
            }
        }
    }

    private void ResetButtonColorsAndEnable()
    {
        // Reset all buttons to their default color and enable them for interaction
        foreach (Button btn in answerButtons)
        {
            SetButtonColor(btn, Color.white); // Reset to original color
            btn.interactable = true; // Enable the button for interaction
        }
    }

    private void DisableAllButtons()
    {
        // Disable all buttons to prevent further interaction
        foreach (Button btn in answerButtons)
        {
            btn.interactable = false;
        }
    }

    // Helper method to set the button's color
    private void SetButtonColor(Button button, Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.disabledColor = color; // Ensure the disabled state keeps the selected color
        button.colors = colors;
    }

    private IEnumerator ShowCorrectAnswerAndNextQuestion()
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds

        // Generate the next question
        FindObjectOfType<NumberManager>().PickRandomNumbers(); // Corrected method call
    }
}
