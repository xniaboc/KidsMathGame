using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SubtractionAnswerManager : MonoBehaviour
{
    public Button[] answerButtons; // Assign the 4 buttons in the Inspector
    private int correctAnswer;
    private int[] answers;

    public void AssignAnswers(int[] answersArray, int correctAns)
    {
        answers = answersArray;
        correctAnswer = correctAns;

        ResetButtonColorsAndEnable();

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answer = answers[i];
            answerButtons[i].GetComponentInChildren<Text>().text = answer.ToString();

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
        FindObjectOfType<SubtractionManager>().ShowCorrectAnswer(correctAnswer);

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

        FindObjectOfType<SubtractionManager>().PickRandomNumbers(); // Generate the next question
    }
}
