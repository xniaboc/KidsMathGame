using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PatternAnswerManager : MonoBehaviour
{
    public Button[] answerButtons;  // Assign the 4 answer buttons in the Inspector
    private int correctAnswer;
    private int[] answers;
    private PatternsManager patternsManager;  // Reference to PatternsManager to call ShowCorrectAnswer()

    public void AssignAnswers(int[] answersArray, int correctAns, PatternsManager manager)
    {
        answers = answersArray;
        correctAnswer = correctAns;
        patternsManager = manager;  // Store the reference to PatternsManager

        // Reset button colors and enable buttons before presenting the new question
        ResetButtonColorsAndEnable();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = answers[i];
            answerButtons[i].GetComponentInChildren<Text>().text = answer.ToString();

            // Clear previous listeners
            answerButtons[i].onClick.RemoveAllListeners();
            int index = i;  // Capture current index for the listener
            answerButtons[i].onClick.AddListener(() => CheckAnswer(answer, index));  // Add listener
        }
    }

    private void CheckAnswer(int selectedAnswer, int buttonIndex)
    {
        // Disable all buttons to prevent multiple clicks
        DisableAllButtons();

        if (selectedAnswer == correctAnswer)
        {
            SetButtonColor(answerButtons[buttonIndex], Color.green);  // Correct answer turns green
        }
        else
        {
            SetButtonColor(answerButtons[buttonIndex], Color.red);  // Incorrect answer turns red
            HighlightCorrectAnswer();  // Highlight correct answer in green
        }

        // Show the correct answer in place of the question mark
        patternsManager.ShowCorrectAnswer();

        // Start coroutine to load the next question after a delay
        StartCoroutine(ShowNextPattern());
    }

    private void HighlightCorrectAnswer()
    {
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (answers[i] == correctAnswer)
            {
                SetButtonColor(answerButtons[i], Color.green);
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
        colors.disabledColor = color;  // Ensure the disabled state keeps the selected color
        button.colors = colors;
    }

    private IEnumerator ShowNextPattern()
    {
        yield return new WaitForSeconds(3);  // Wait 3 seconds before loading the next pattern
        patternsManager.GeneratePattern();  // Load a new pattern
    }
}
