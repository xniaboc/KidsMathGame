using UnityEngine;
using System.Collections.Generic;

public class DivisionManager : MonoBehaviour
{
    public GameObject[] numberPrefabs;  // Prefabs for numbers 0-9
    public GameObject divisionSignPrefab;
    public GameObject equalSignPrefab;
    public DivisionAnswerManager answerManager;

    private List<GameObject> instantiatedPrefabs = new List<GameObject>(); // Track instantiated prefabs
    private List<(int dividend, int divisor)> validEquations = new List<(int, int)>(); // Store valid equations
    private int currentDividend;
    private int currentDivisor;
    private int correctAnswer;

    void Start()
    {
        // Precompute all valid division equations
        GenerateValidEquations();

        // Automatically pick numbers when the game starts
        PickRandomNumbers();
    }

    // Precompute all valid division equations
    void GenerateValidEquations()
    {
        // Loop through all possible divisors and dividends from 1 to 99
        for (int divisor = 1; divisor <= 99; divisor++)
        {
            for (int dividend = divisor; dividend <= 99; dividend += divisor)
            {
                validEquations.Add((dividend, divisor));
            }
        }

        Debug.Log($"Total valid equations: {validEquations.Count}");
    }

    public void PickRandomNumbers()
    {
        Debug.Log("Picking random numbers...");
        DestroyOldPrefabs();

        // Randomly pick a valid division equation from the list
        var equation = validEquations[Random.Range(0, validEquations.Count)];
        currentDividend = equation.dividend;
        currentDivisor = equation.divisor;

        // Log generated equation for debugging
        Debug.Log($"Generated division equation: {currentDividend} ÷ {currentDivisor}");

        // Instantiate the prefabs for the equation (e.g., 35 ÷ 7)
        InstantiateEquation(currentDividend, currentDivisor);

        // Correct answer is currentDividend ÷ currentDivisor
        correctAnswer = currentDividend / currentDivisor;
        Debug.Log($"Correct answer: {correctAnswer}");

        // Generate the answer options including the correct one
        List<int> possibleAnswers = GenerateAnswers(correctAnswer);

        // Pass the answers to the answer manager to assign to buttons
        answerManager.AssignAnswers(possibleAnswers.ToArray(), correctAnswer);
    }

    private void InstantiateEquation(int dividend, int divisor)
    {
        // Check if the dividend is one digit or two digits
        if (dividend >= 10)
        {
            // Two digits dividend
            InstantiateDoubleDigitNumber(dividend, new Vector3(9.0f, 0, 0), new Vector3(6.5f, 0, 0));
        }
        else
        {
            // Single digit dividend, place at X = 6.0
            InstantiateDoubleDigitNumber(dividend, new Vector3(6.0f, 0, 0), Vector3.zero);
        }

        // Instantiate the divisor (right side number)
        if (divisor >= 10)
        {
            // Two digits divisor
            InstantiateDoubleDigitNumber(divisor, new Vector3(-0.5f, 0, 0), new Vector3(-3.0f, 0, 0));
            // Set the equals sign at -6.0 if divisor is two digits
            instantiatedPrefabs.Add(Instantiate(equalSignPrefab, new Vector3(-6.0f, 2, 0), Quaternion.identity));
        }
        else
        {
            // Single digit divisor
            InstantiateDoubleDigitNumber(divisor, new Vector3(-0.5f, 0, 0), Vector3.zero);
            // Set the equals sign at -3.0 if divisor is one digit
            instantiatedPrefabs.Add(Instantiate(equalSignPrefab, new Vector3(-3.0f, 2, 0), Quaternion.identity));
        }

        // Instantiate the division sign
        instantiatedPrefabs.Add(Instantiate(divisionSignPrefab, new Vector3(3.0f, 2, 0), Quaternion.identity));
    }


    // New method to instantiate the answer prefabs only after the player has made a guess
    public void ShowCorrectAnswer()
    {
        // Display the correct answer after the equals sign
        InstantiateAnswer(correctAnswer, currentDivisor);
    }

    private void InstantiateAnswer(int answer, int divisor)
    {
        // Determine positions based on the divisor size
        if (divisor >= 10)
        {
            // Two digits divisor
            if (answer >= 10)
            {
                // Two-digit answer
                InstantiateDoubleDigitNumber(answer, new Vector3(-9.0f, 0, 0), new Vector3(-11.5f, 0, 0));
            }
            else
            {
                // Single-digit answer
                InstantiateDoubleDigitNumber(answer, new Vector3(-9.0f, 0, 0), Vector3.zero);
            }
        }
        else
        {
            // Single-digit divisor
            if (answer >= 10)
            {
                // Two-digit answer
                InstantiateDoubleDigitNumber(answer, new Vector3(-7.0f, 0, 0), new Vector3(-10.0f, 0, 0));
            }
            else
            {
                // Single-digit answer
                InstantiateDoubleDigitNumber(answer, new Vector3(-7.0f, 0, 0), Vector3.zero);
            }
        }
    }

    private void InstantiateDoubleDigitNumber(int number, Vector3 tensPosition, Vector3 onesPosition)
    {
        Debug.Log($"Instantiating number: {number}");

        if (number >= 10)
        {
            // Split into two digits for tens and ones
            int tens = number / 10;
            int ones = number % 10;

            GameObject tensPrefab = FindPrefabByNumber(tens);
            GameObject onesPrefab = FindPrefabByNumber(ones);

            if (tensPrefab != null && onesPrefab != null)
            {
                // Tens place and ones place
                instantiatedPrefabs.Add(Instantiate(tensPrefab, tensPosition, Quaternion.identity)); // Tens place
                instantiatedPrefabs.Add(Instantiate(onesPrefab, onesPosition, Quaternion.identity)); // Ones place
            }
            else
            {
                Debug.LogError($"Prefab not found for tens: {tens} or ones: {ones}. Skipping instantiation.");
            }
        }
        else
        {
            // Single digit case
            GameObject singleDigitPrefab = FindPrefabByNumber(number);
            if (singleDigitPrefab != null)
            {
                instantiatedPrefabs.Add(Instantiate(singleDigitPrefab, tensPosition, Quaternion.identity)); // Single digit
            }
            else
            {
                Debug.LogError($"Prefab for single digit {number} not found! Skipping instantiation.");
            }
        }
    }

    private GameObject FindPrefabByNumber(int number)
    {
        string prefabName = number.ToString();
        foreach (GameObject prefab in numberPrefabs)
        {
            if (prefab.name.StartsWith(prefabName))  // Match the prefab name with the number
            {
                return prefab;
            }
        }
        Debug.LogError($"Prefab for number {number} not found!");
        return null;
    }

    private List<int> GenerateAnswers(int correctAnswer)
    {
        List<int> answers = new List<int>();
        answers.Add(correctAnswer);

        int attempts = 0; // Add a counter for safety
        while (answers.Count < 4 && attempts < 100)  // Prevent endless attempts
        {
            int randomAnswer = correctAnswer + Random.Range(-3, 4);  // Randomly close to the correct answer
            if (randomAnswer >= 0 && randomAnswer <= 50 && !answers.Contains(randomAnswer))  // Ensure valid range and no duplicates
            {
                answers.Add(randomAnswer);
            }
            attempts++;  // Increment counter
        }

        if (attempts >= 100)
        {
            Debug.LogError("Exceeded max attempts in generating random answers.");
        }

        Debug.Log($"Generated answers: {string.Join(", ", answers)}");

        // Shuffle answers to randomize their order
        Shuffle(answers);
        return answers;
    }

    private void DestroyOldPrefabs()
    {
        Debug.Log("Destroying old prefabs...");
        foreach (GameObject prefab in instantiatedPrefabs)
        {
            if (prefab != null)
            {
                Destroy(prefab);
            }
            else
            {
                Debug.LogError("Encountered null prefab in instantiatedPrefabs list.");
            }
        }
        instantiatedPrefabs.Clear();
        Debug.Log("Old prefabs destroyed.");
    }

    private void Shuffle(List<int> array)
    {
        for (int i = 0; i < array.Count; i++)
        {
            int randomIndex = Random.Range(0, array.Count);
            int temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}

