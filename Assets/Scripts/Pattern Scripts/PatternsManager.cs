using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PatternsManager : MonoBehaviour
{
    public GameObject[] numberPrefabs;  // Ensure all number prefabs are assigned
    public GameObject questionMarkPrefab;  // For the '?' in the pattern
    public GameObject dotPrefab;  // For the dots separating the numbers
    public PatternAnswerManager answerManager;  // Assign the PatternAnswerManager

    private List<GameObject> instantiatedPrefabs = new List<GameObject>();
    private int correctAnswer;
    private Vector3 questionMarkPosition;  // Track the position of the '?' mark

    public void Start()
    {
        GeneratePattern();  // Generate the first pattern when the game starts
    }

    public void GeneratePattern()
    {
        // Ensure old prefabs are destroyed before generating a new pattern
        DestroyOldPrefabs();

        // Randomly generate a new pattern (random increments)
        List<int> currentPattern = GenerateRandomPattern(out correctAnswer);

        // Instantiate the pattern on screen
        InstantiatePattern(currentPattern);

        // Generate possible answer options
        GenerateAnswerOptions(correctAnswer);
    }

    private List<int> GenerateRandomPattern(out int correctAnswer)
    {
        // Generate random pattern sequence (e.g., 2, 4, 6, ?, 10)
        int start = Random.Range(1, 10);  // Random start number
        int step = Random.Range(1, 5);  // Random step size
        int length = 4;  // Length of the pattern (e.g., 2, 4, 6, ?, 10)

        List<int> pattern = new List<int>();
        for (int i = 0; i < length - 1; i++)
        {
            pattern.Add(start + step * i);  // Add the numbers of the pattern
        }

        // Correct answer is the missing number (between 2nd last and last number)
        correctAnswer = start + step * (length - 1);
        pattern.Add(correctAnswer + step);  // Add final number (which is not missing)

        return pattern;
    }

    private void InstantiatePattern(List<int> pattern)
    {
        float startX = 7.0f;  // Starting X position for the first number
        float spacing = -4.0f;  // Spacing between elements

        // Instantiate the numbers and dots
        for (int i = 0; i < pattern.Count; i++)
        {
            if (i < pattern.Count - 1)  // Add numbers with dots between them
            {
                InstantiateNumber(pattern[i], new Vector3(startX, 0, 0));
                instantiatedPrefabs.Add(Instantiate(dotPrefab, new Vector3(startX + spacing / 2, 0, 0), Quaternion.identity));
            }
            else  // Last number is replaced with a question mark
            {
                questionMarkPosition = new Vector3(startX, 0, 0);  // Store the position of the '?'
                instantiatedPrefabs.Add(Instantiate(questionMarkPrefab, questionMarkPosition, Quaternion.identity));
            }

            startX += spacing;  // Move to the next position
        }
    }

    // Method to instantiate single or double digit numbers using prefabs
    private void InstantiateNumber(int number, Vector3 position)
    {
        if (number >= 10)  // Handle double-digit numbers
        {
            int tens = number / 10;  // Tens digit
            int ones = number % 10;  // Ones digit

            // Tens digit goes on the left, ones on the right
            InstantiateNumberPrefab(tens, new Vector3(position.x, position.y, 0));  // Tens digit first
            InstantiateNumberPrefab(ones, new Vector3(position.x - 1.5f, position.y, 0));  // Ones digit second
        }
        else  // Single digit numbers
        {
            InstantiateNumberPrefab(number, position);  // Instantiate single digit number
        }
    }

    private void InstantiateNumberPrefab(int digit, Vector3 position)
    {
        string prefabName = digit.ToString();
        GameObject selectedPrefab = FindPrefabByName(prefabName);

        if (selectedPrefab != null)
        {
            instantiatedPrefabs.Add(Instantiate(selectedPrefab, position, Quaternion.identity));
        }
        else
        {
            Debug.LogError($"Prefab for number {digit} not found!");
        }
    }

    private GameObject FindPrefabByName(string prefabName)
    {
        foreach (GameObject prefab in numberPrefabs)
        {
            if (prefab.name == prefabName)
            {
                return prefab;
            }
        }

        Debug.LogError($"Prefab with name {prefabName} not found!");
        return null;
    }

    private void GenerateAnswerOptions(int correctAnswer)
    {
        int[] answers = new int[4];
        answers[0] = correctAnswer;  // Correct answer as the first option

        // Generate 3 close wrong answers
        for (int i = 1; i < 4; i++)
        {
            int wrongAnswer;
            do
            {
                wrongAnswer = correctAnswer + Random.Range(-3, 3);  // Randomize around the correct answer
            } while (wrongAnswer == correctAnswer || System.Array.Exists(answers, element => element == wrongAnswer));  // Ensure no duplicates
            answers[i] = wrongAnswer;
        }

        // Shuffle the answers
        Shuffle(answers);

        // Pass the answers to the answer manager
        answerManager.AssignAnswers(answers, correctAnswer, this);  // Pass PatternsManager reference to allow showing the correct answer
    }

    // Method to show the correct answer in place of the question mark
    public void ShowCorrectAnswer()
    {
        // Move the correct answer slightly to the right of the question mark
        Vector3 adjustedPosition = new Vector3(questionMarkPosition.x - 2.0f, questionMarkPosition.y, questionMarkPosition.z);
        InstantiateNumber(correctAnswer, adjustedPosition);
    }

    private void Shuffle(int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(0, array.Length);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    private void DestroyOldPrefabs()
    {
        // Destroy all previously instantiated prefabs
        foreach (GameObject prefab in instantiatedPrefabs)
        {
            Destroy(prefab);
        }
        instantiatedPrefabs.Clear();  // Clear the list after destruction
    }
}
