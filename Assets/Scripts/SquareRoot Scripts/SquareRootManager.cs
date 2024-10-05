using UnityEngine;
using System.Collections.Generic;

public class SquareRootManager : MonoBehaviour
{
    public GameObject[] numberPrefabs; // Ensure all 10 prefabs (0-9) are assigned
    public GameObject squareRootSymbolPrefab;
    public GameObject equalSignPrefab;
    public SquareRootAnswerManager answerManager;

    private List<GameObject> instantiatedPrefabs = new List<GameObject>();
    private int currentSquareRoot;

    // List of valid perfect squares (1-900)
    private int[] perfectSquares = new int[] {
        1, 4, 9, 16, 25, 36, 49, 64, 81, 100, 121, 144, 169, 196, 225, 256, 289, 324,
        361, 400, 441, 484, 529, 576, 625, 676, 729, 784, 841, 900
    };

    void Start()
    {
        // Automatically pick numbers when the game starts
        PickRandomSquareRoot();
    }

    public void PickRandomSquareRoot()
    {
        DestroyOldPrefabs();

        // Select a random perfect square from the list
        int randomIndex = Random.Range(0, perfectSquares.Length);
        currentSquareRoot = perfectSquares[randomIndex]; // This ensures only perfect squares are chosen

        // Ensure the value is valid by logging it
        Debug.Log($"Picked a perfect square: {currentSquareRoot}");

        // Calculate the correct answer (square root)
        int correctAnswer = Mathf.RoundToInt(Mathf.Sqrt(currentSquareRoot));

        // Log the correct answer for debugging
        Debug.Log($"Square root of {currentSquareRoot} is {correctAnswer}");

        // Instantiate the equation: √currentSquareRoot =
        InstantiateEquation(currentSquareRoot);

        // Generate the answer options including the correct one
        List<int> possibleAnswers = GenerateAnswers(correctAnswer);

        // Pass the answers to the answer manager to assign to buttons
        answerManager.AssignAnswers(possibleAnswers.ToArray(), correctAnswer);
    }

    private void InstantiateEquation(int squareRoot)
    {
        // Instantiate the square root symbol at X = 7.0
        instantiatedPrefabs.Add(Instantiate(squareRootSymbolPrefab, new Vector3(4.0f, 2.8f, 0), Quaternion.identity));

        // Log the square root number for debugging
        Debug.Log($"Instantiating number: {squareRoot}");

        // Instantiate the square root number (split into individual digits if necessary)
        InstantiateNumberAsPrefabs(squareRoot, new Vector3(3.0f, 0, 0));

        // Instantiate the equals sign at X = -4.5
        instantiatedPrefabs.Add(Instantiate(equalSignPrefab, new Vector3(-4.50f, 2, 0), Quaternion.identity));
    }

    public void ShowCorrectAnswer(int correctAnswer)
    {
        // Destroy any previous answer prefabs to avoid overlaps
        DestroyOldPrefabsAfterEquation();

        // Show the correct answer (single-digit or two-digit)
        InstantiateNumberAsPrefabs(correctAnswer, new Vector3(-8.0f, 0, 0));
    }

    private void InstantiateNumberAsPrefabs(int number, Vector3 startPosition)
    {
        // Handle three-digit numbers by splitting them into three prefabs
        if (number >= 100)
        {
            int hundreds = number / 100;
            int tens = (number % 100) / 10;
            int ones = number % 10;

            Debug.Log($"Instantiating three-digit number: {hundreds}{tens}{ones}");

            InstantiateNumber(hundreds, startPosition);                           // Hundreds digit
            InstantiateNumber(tens, startPosition + new Vector3(-2.5f, 0, 0));    // Tens digit
            InstantiateNumber(ones, startPosition + new Vector3(-5.0f, 0, 0));    // Ones digit
        }
        // Handle two-digit numbers
        else if (number >= 10)
        {
            int tens = number / 10;
            int ones = number % 10;

            Debug.Log($"Instantiating two-digit number: {tens}{ones}");

            InstantiateNumber(tens, startPosition);                   // Tens digit
            InstantiateNumber(ones, startPosition + new Vector3(-2.5f, 0, 0)); // Ones digit
        }
        else
        {
            // For single-digit numbers
            Debug.Log($"Instantiating single-digit number: {number}");
            InstantiateNumber(number, startPosition);
        }
    }

    private void InstantiateNumber(int number, Vector3 position)
    {
        GameObject selectedPrefab = FindPrefabByNumber(number);

        if (selectedPrefab != null)
        {
            instantiatedPrefabs.Add(Instantiate(selectedPrefab, position, Quaternion.identity));
        }
        else
        {
            Debug.LogError($"Prefab for number {number} not found!"); // Log an error if a prefab is missing
        }
    }

    private GameObject FindPrefabByNumber(int number)
    {
        string prefabName = number.ToString();

        foreach (GameObject prefab in numberPrefabs)
        {
            if (prefab.name.StartsWith(prefabName))
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

        // Add the correct answer first (the square root of the currentSquareRoot)
        answers.Add(correctAnswer);

        // Generate 3 incorrect answers that are not equal to the correct answer
        while (answers.Count < 4)
        {
            int randomAnswer = correctAnswer + Random.Range(-3, 4);
            if (randomAnswer >= 1 && randomAnswer <= 30 && !answers.Contains(randomAnswer))
            {
                answers.Add(randomAnswer);
            }
        }

        Shuffle(answers); // Shuffle the answers to randomize their order
        return answers;
    }

    private void DestroyOldPrefabs()
    {
        foreach (GameObject prefab in instantiatedPrefabs)
        {
            Destroy(prefab);
        }
        instantiatedPrefabs.Clear();
    }

    private void DestroyOldPrefabsAfterEquation()
    {
        // Remove only the prefabs related to the answer
        List<GameObject> prefabsToRemove = new List<GameObject>();
        foreach (GameObject prefab in instantiatedPrefabs)
        {
            if (prefab.transform.position.x <= -8.0f) // Answer prefabs
            {
                prefabsToRemove.Add(prefab);
            }
        }
        foreach (GameObject prefab in prefabsToRemove)
        {
            instantiatedPrefabs.Remove(prefab);
            Destroy(prefab);
        }
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
