using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DivisionAnswerManager : MonoBehaviour
{
    public Button[] answerButtons; // Assign the 4 buttons in the Inspector
    private int correctAnswer;
    private int[] answers; // Store the answers

    public void AssignAnswers(int[] answersArray, int correctAns)
    {
        // Ensure we have exactly 4 answers
        if (answersArray.Length != 4)
        {
            Debug.LogError($"Expected 4 answers, but got {answersArray.Length}");
            return; // Exit if we don't have 4 answers to prevent index issues
        }

        // Ensure we have 4 buttons assigned in the inspector
        if (answerButtons.Length != 4)
        {
            Debug.LogError($"Expected 4 buttons, but found {answerButtons.Length}.");
            return; // Exit if there aren't 4 buttons
        }

        answers = answersArray;
        correctAnswer = correctAns;

        // Log assigned answers and correct answer for debugging
        Debug.Log($"Assigned answers: {string.Join(", ", answersArray)}");
        Debug.Log($"Correct answer is: {correctAns}");

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
        // Disable all buttons to prevent multiple clicks
        DisableAllButtons();

        // Log the selected answer and check if it is correct
        Debug.Log($"Selected answer: {selectedAnswer}, Button Index: {buttonIndex}");

        // Check if the selected answer is correct
        if (selectedAnswer == correctAnswer)
        {
            Debug.Log("Correct answer selected.");
            // Highlight the clicked button green for correct
            SetButtonColor(answerButtons[buttonIndex], Color.green);
        }
        else
        {
            Debug.Log("Incorrect answer selected.");
            // Highlight the clicked button red for incorrect
            SetButtonColor(answerButtons[buttonIndex], Color.red);
            HighlightCorrectAnswer();
        }

        // Call the DivisionManager to display the correct answer prefabs after the player selects
        FindObjectOfType<DivisionManager>().ShowCorrectAnswer();

        // Start coroutine to handle question transition
        StartCoroutine(ShowCorrectAnswerAndNextQuestion());
    }

    private void HighlightCorrectAnswer()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (answers[i] == correctAnswer)
            {
                SetButtonColor(answerButtons[i], Color.green);
                Debug.Log($"Highlighting correct answer button at index: {i}");
            }
        }
    }

    private void ResetButtonColorsAndEnable()
    {
        foreach (Button btn in answerButtons)
        {
            SetButtonColor(btn, Color.white);
            btn.interactable = true;
        }
    }

    private void DisableAllButtons()
    {
        foreach (Button btn in answerButtons)
        {
            btn.interactable = false;
        }
    }

    private void SetButtonColor(Button button, Color color)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = color;
        colors.disabledColor = color;
        button.colors = colors;
    }

    private IEnumerator ShowCorrectAnswerAndNextQuestion()
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds

        FindObjectOfType<DivisionManager>().PickRandomNumbers(); // Generate the next question
    }
}
