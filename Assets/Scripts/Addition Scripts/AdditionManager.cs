using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AdditionManager : MonoBehaviour
{
    public GameObject[] numberPrefabs;  // Assign all number prefabs in the Inspector
    public GameObject plusSignPrefab;   // Assign the "+" sign prefab in the Inspector
    public GameObject equalSignPrefab;  // Assign the "=" sign prefab in the Inspector
    public AdditionAnswerManager answerManager; // Assign the AdditionAnswerManager in the Inspector

    public Button[] numberButtons;      // Assign buttons 0-9 in the Inspector
    public Button allButton;            // Assign the "ALL" button in the Inspector
    public Toggle easyModeToggle;       // Assign the toggle in the Inspector for easy mode
    public Color activeColor = Color.green; // Color to indicate active button
    public Color inactiveColor = Color.white; // Color for inactive buttons

    private List<int> selectedNumbers = new List<int>(); // Track selected numbers
    private List<GameObject> instantiatedPrefabs = new List<GameObject>(); // Track instantiated prefabs
    private int currentNumber1; // Store the first number of the equation
    private int currentNumber2; // Store the second number of the equation

    void Start()
    {
        // Start with "ALL" selected
        SelectAllNumbers();

        // Set up the buttons programmatically
        SetupNumberButtons();

        // Automatically pick numbers when the game starts
        PickRandomNumbers();

        // Add a listener to the toggle to check for changes
        if (easyModeToggle != null)
        {
            easyModeToggle.onValueChanged.AddListener(delegate {
                ValidateCurrentEquation();
            });
        }
    }

    private void SetupNumberButtons()
    {
        for (int i = 0; i < numberButtons.Length; i++)
        {
            int number = i;
            numberButtons[i].onClick.AddListener(() => ToggleNumber(number));
        }

        allButton.onClick.AddListener(SelectAllNumbers);

        HighlightButton(allButton, true);
        foreach (Button button in numberButtons)
        {
            HighlightButton(button, false);
        }
    }

    private void HighlightButton(Button button, bool isActive)
    {
        ColorBlock colors = button.colors;
        colors.normalColor = isActive ? activeColor : inactiveColor;
        colors.selectedColor = isActive ? activeColor : inactiveColor;
        button.colors = colors;
    }

    public void SelectAllNumbers()
    {
        selectedNumbers.Clear();
        for (int i = 0; i <= 9; i++)
        {
            selectedNumbers.Add(i);
        }

        HighlightButton(allButton, true);
        foreach (Button button in numberButtons)
        {
            HighlightButton(button, false);
        }

        PickRandomNumbers();
    }

    public void ToggleNumber(int number)
    {
        if (selectedNumbers.Count == 10)
        {
            selectedNumbers.Clear();
        }

        HighlightButton(allButton, false);

        if (selectedNumbers.Contains(number))
        {
            selectedNumbers.Remove(number);
            HighlightButton(numberButtons[number], false);
        }
        else
        {
            selectedNumbers.Add(number);
            HighlightButton(numberButtons[number], true);
        }

        if (selectedNumbers.Count == 10)
        {
            SelectAllNumbers();
        }
        else if (selectedNumbers.Count == 0)
        {
            SelectAllNumbers();
        }

        RefreshEquationIfNecessary();
    }

    private void RefreshEquationIfNecessary()
    {
        if (!selectedNumbers.Contains(currentNumber1))
        {
            PickRandomNumbers();
        }
    }

    private void ValidateCurrentEquation()
    {
        if (easyModeToggle != null && easyModeToggle.isOn)
        {
            if (selectedNumbers.Count == 10 && !(currentNumber1 <= 5 && currentNumber2 <= 5))
            {
                PickRandomNumbers();
            }
            else if (!(currentNumber1 <= 5 || currentNumber2 <= 5))
            {
                PickRandomNumbers();
            }
        }
    }

    public void PickRandomNumbers()
    {
        DestroyOldPrefabs();

        if (selectedNumbers.Count == 0)
        {
            return;
        }

        int number1;
        int number2;

        bool isAllSelected = selectedNumbers.Count == 10;

        if (easyModeToggle != null && easyModeToggle.isOn && isAllSelected)
        {
            number1 = Random.Range(0, 6);
            number2 = Random.Range(0, 6);
        }
        else if (easyModeToggle != null && easyModeToggle.isOn)
        {
            number1 = selectedNumbers[Random.Range(0, selectedNumbers.Count)];
            number2 = Random.Range(0, 6);
        }
        else
        {
            number1 = selectedNumbers[Random.Range(0, selectedNumbers.Count)];
            number2 = Random.Range(0, 10);
        }

        currentNumber1 = number1;
        currentNumber2 = number2;

        InstantiateEquation(number1, number2);

        int correctAnswer = number1 + number2;
        GenerateAnswerOptions(correctAnswer);
    }

    private void InstantiateEquation(int number1, int number2)
    {
        // Instantiate first number at X = 7.0
        InstantiateNumber(number1, new Vector3(7.0f, 0, 0));

        // Instantiate plus sign at X = 3.0
        instantiatedPrefabs.Add(Instantiate(plusSignPrefab, new Vector3(3.0f, 2, 0), Quaternion.identity));

        // Instantiate second number at X = -1.0
        InstantiateNumber(number2, new Vector3(-1.0f, 0, 0));

        // Instantiate equals sign at X = -4.50
        instantiatedPrefabs.Add(Instantiate(equalSignPrefab, new Vector3(-4.50f, 2, 0), Quaternion.identity));
    }

    public void ShowCorrectAnswer(int correctAnswer)
    {
        if (correctAnswer >= 10)
        {
            int tens = correctAnswer / 10;
            int ones = correctAnswer % 10;

            InstantiateNumber(tens, new Vector3(-8.0f, 0, 0));
            InstantiateNumber(ones, new Vector3(-11.50f, 0, 0));
        }
        else
        {
            InstantiateNumber(correctAnswer, new Vector3(-8.0f, 0, 0));
        }
    }

    private void InstantiateNumber(int number, Vector3 position)
    {
        string prefabName = number.ToString();
        GameObject selectedPrefab = FindPrefabByName(prefabName);

        if (selectedPrefab != null)
        {
            instantiatedPrefabs.Add(Instantiate(selectedPrefab, position, Quaternion.identity));
        }
    }

    private void DestroyOldPrefabs()
    {
        foreach (GameObject prefab in instantiatedPrefabs)
        {
            Destroy(prefab);
        }
        instantiatedPrefabs.Clear();
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
        return null;
    }

    private void GenerateAnswerOptions(int correctAnswer)
    {
        int[] answers = new int[4];
        answers[0] = correctAnswer;

        for (int i = 1; i < 4; i++)
        {
            int wrongAnswer;
            do
            {
                wrongAnswer = Random.Range(0, 19);
            } while (wrongAnswer == correctAnswer || System.Array.Exists(answers, element => element == wrongAnswer));
            answers[i] = wrongAnswer;
        }

        Shuffle(answers);

        answerManager.AssignAnswers(answers, correctAnswer);
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
}
