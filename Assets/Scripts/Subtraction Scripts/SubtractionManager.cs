using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SubtractionManager : MonoBehaviour
{
    public GameObject[] numberPrefabs; // Ensure all 30 prefabs (0-9 and animations) are assigned
    public GameObject minusSignPrefab;
    public GameObject equalSignPrefab;
    public SubtractionAnswerManager answerManager;

    private List<GameObject> instantiatedPrefabs = new List<GameObject>();
    private int currentEquationNumber1;
    private int currentEquationNumber2;

    void Start()
    {
        // Automatically pick numbers when the game starts
        PickRandomNumbers();
    }

    public void PickRandomNumbers()
    {
        DestroyOldPrefabs();

        // Generate two random numbers between 0 and 9
        int number1 = Random.Range(0, 10);
        int number2 = Random.Range(0, 10);

        // Ensure number1 is larger or equal to number2 to avoid negative results
        if (number2 > number1)
        {
            int temp = number1;
            number1 = number2;
            number2 = temp;
        }

        currentEquationNumber1 = number1;
        currentEquationNumber2 = number2;

        // Instantiate the prefabs for the equation (number1 - number2)
        InstantiateEquation(number1, number2);

        // Correct answer is number1 - number2
        int correctAnswer = number1 - number2;

        // Generate the answer options including the correct one
        List<int> possibleAnswers = GenerateAnswers(correctAnswer);

        // Pass the answers to the answer manager to assign to buttons
        answerManager.AssignAnswers(possibleAnswers.ToArray(), correctAnswer);
    }

    private void InstantiateEquation(int number1, int number2)
    {
        // Instantiate the first number at X = 7.0
        InstantiateNumber(number1, new Vector3(7.0f, 0, 0));

        // Instantiate the minus sign at X = 3.0
        instantiatedPrefabs.Add(Instantiate(minusSignPrefab, new Vector3(3.0f, 2, 0), Quaternion.identity));

        // Instantiate the second number at X = -1.0
        InstantiateNumber(number2, new Vector3(-1.0f, 0, 0));

        // Instantiate the equals sign at X = -4.5
        instantiatedPrefabs.Add(Instantiate(equalSignPrefab, new Vector3(-4.50f, 2, 0), Quaternion.identity));
    }

    public void ShowCorrectAnswer(int correctAnswer)
    {
        // Destroy any previous answer prefabs to avoid overlaps
        DestroyOldPrefabsAfterEquation();

        if (correctAnswer >= 10)
        {
            // Two-digit answer: tens at X = -8.0, ones at X = -11.50
            int tens = correctAnswer / 10;
            int ones = correctAnswer % 10;

            InstantiateNumber(tens, new Vector3(-8.0f, 0, 0));  // Tens digit of the answer at X = -8.0
            InstantiateNumber(ones, new Vector3(-11.50f, 0, 0));  // Ones digit of the answer at X = -11.50
        }
        else
        {
            // One-digit answer at X = -8.0
            InstantiateNumber(correctAnswer, new Vector3(-8.0f, 0, 0));
        }
    }

    private void InstantiateNumber(int number, Vector3 position)
    {
        string prefabName = number.ToString();
        GameObject selectedPrefab = FindPrefabByNumber(number);

        if (selectedPrefab != null)
        {
            instantiatedPrefabs.Add(Instantiate(selectedPrefab, position, Quaternion.identity));
        }
        else
        {
            Debug.LogError("Prefab for number " + number + " not found!");
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

        // Add the correct answer first
        answers.Add(correctAnswer);

        // Generate 3 reasonable incorrect answers that are not equal to the correct answer
        while (answers.Count < 4)
        {
            int randomAnswer = correctAnswer + Random.Range(-3, 4);
            if (randomAnswer >= 0 && !answers.Contains(randomAnswer))
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
