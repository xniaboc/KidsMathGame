using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SquareRootAnswerManager : MonoBehaviour
{
    public Button[] answerButtons; // Assign the 4 buttons in the Inspector
    private int correctAnswer;
    private int[] answers;
    private SquareRootManager squareRootManager; // Cached reference to avoid repeated FindObjectOfType calls

    void Start()
    {
        // Cache the SquareRootManager reference for better performance
        squareRootManager = FindObjectOfType<SquareRootManager>();

        if (squareRootManager == null)
        {
            Debug.LogError("SquareRootManager not found in the scene!");
        }

        if (answerButtons == null || answerButtons.Length == 0)
        {
            Debug.LogError("Answer buttons are not properly assigned in the Inspector!");
        }
    }

    public void AssignAnswers(int[] answersArray, int correctAns)
    {
        answers = answersArray;
        correctAnswer = correctAns;

        ResetButtonColorsAndEnable();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = answers[i];
            Text buttonText = answerButtons[i].GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.text = answer.ToString();
            }
            else
            {
                Debug.LogError("Text component missing on answer button!");
            }

            answerButtons[i].onClick.RemoveAllListeners();
            int index = i;
            answerButtons[i].onClick.AddListener(() => CheckAnswer(answer, index));
        }
    }

    private void CheckAnswer(int selectedAnswer, int buttonIndex)
    {
        DisableAllButtons();

        if (selectedAnswer == correctAnswer)
        {
            SetButtonColor(answerButtons[buttonIndex], Color.green);
        }
        else
        {
            SetButtonColor(answerButtons[buttonIndex], Color.red);
            HighlightCorrectAnswer();
        }

        // Show the correct answer using prefabs
        if (squareRootManager != null)
        {
            squareRootManager.ShowCorrectAnswer(correctAnswer);
        }
        else
        {
            Debug.LogError("SquareRootManager reference is missing!");
        }

        StartCoroutine(ShowCorrectAnswerAndNextQuestion());
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
        colors.disabledColor = color;
        button.colors = colors;
    }

    private IEnumerator ShowCorrectAnswerAndNextQuestion()
    {
        yield return new WaitForSeconds(3); // Wait for 3 seconds

        if (squareRootManager != null)
        {
            squareRootManager.PickRandomSquareRoot(); // Generate the next question
        }
    }
}
