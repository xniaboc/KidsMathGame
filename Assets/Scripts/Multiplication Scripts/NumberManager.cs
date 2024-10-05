using UnityEngine;
using UnityEngine.UI; // For UI components like Toggle and Button
using System.Collections.Generic; // For List support

public class NumberManager : MonoBehaviour
{
    public GameObject[] numberPrefabs;  // Assign all 30 number prefabs in the Inspector
    public GameObject xSignPrefab;      // Assign the X sign prefab in the Inspector
    public GameObject equalSignPrefab;  // Assign the = sign prefab in the Inspector
    public AnswerManager answerManager; // Assign the AnswerManager in the Inspector

    public Button[] tableButtons;       // Assign buttons 0-9 in the Inspector
    public Button allButton;            // Assign the "ALL" button in the Inspector
    public Toggle easyModeToggle;       // Assign the toggle in the Inspector for easy mode
    public Color activeColor = Color.green; // Color to indicate active button
    public Color inactiveColor = Color.white; // Color for inactive buttons

    public List<int> selectedTimesTables = new List<int>(); // Track selected times tables
    private List<GameObject> instantiatedPrefabs = new List<GameObject>(); // Track instantiated prefabs
    private int currentEquationNumber1; // Store the first number of the equation
    private int currentEquationNumber2; // Store the second number of the equation

    void Start()
    {
        // Start with "ALL" selected
        SelectAllTimesTables();

        // Set up the buttons programmatically
        SetupTableButtons();

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

    private void SetupTableButtons()
    {
        for (int i = 0; i < tableButtons.Length; i++)
        {
            int tableNumber = i;
            tableButtons[i].onClick.AddListener(() => ToggleTimesTable(tableNumber));
        }

        allButton.onClick.AddListener(SelectAllTimesTables);

        HighlightButton(allButton, true);
        foreach (Button button in tableButtons)
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

    public void SelectAllTimesTables()
    {
        selectedTimesTables.Clear();
        for (int i = 0; i <= 9; i++)
        {
            selectedTimesTables.Add(i);
        }

        HighlightButton(allButton, true);
        foreach (Button button in tableButtons)
        {
            HighlightButton(button, false);
        }

        Debug.Log("All times tables selected.");
    }

    public void ToggleTimesTable(int tableNumber)
    {
        if (selectedTimesTables.Count == 10)
        {
            selectedTimesTables.Clear();
        }

        HighlightButton(allButton, false);

        if (selectedTimesTables.Contains(tableNumber))
        {
            selectedTimesTables.Remove(tableNumber);
            HighlightButton(tableButtons[tableNumber], false);
            Debug.Log("Removed times table: " + tableNumber);
        }
        else
        {
            selectedTimesTables.Add(tableNumber);
            HighlightButton(tableButtons[tableNumber], true);
            Debug.Log("Added times table: " + tableNumber);
        }

        if (selectedTimesTables.Count == 10)
        {
            SelectAllTimesTables();
            return;
        }

        if (selectedTimesTables.Count == 0)
        {
            SelectAllTimesTables();
            return;
        }

        RefreshEquationIfNecessary();
    }

    private void RefreshEquationIfNecessary()
    {
        if (!selectedTimesTables.Contains(currentEquationNumber1))
        {
            Debug.Log("Current equation doesn't match the selected times tables. Refreshing...");
            PickRandomNumbers();
        }
    }

    private void ValidateCurrentEquation()
    {
        if (easyModeToggle != null && easyModeToggle.isOn)
        {
            if (selectedTimesTables.Count == 10 && !(currentEquationNumber1 <= 5 && currentEquationNumber2 <= 5))
            {
                Debug.Log("Current equation doesn't meet easy mode criteria for ALL. Refreshing...");
                PickRandomNumbers();
            }
            else if (!(currentEquationNumber1 <= 5 || currentEquationNumber2 <= 5))
            {
                Debug.Log("Current equation doesn't meet easy mode criteria. Refreshing...");
                PickRandomNumbers();
            }
        }
    }

    public void PickRandomNumbers()
    {
        Debug.Log("PickRandomNumbers() is running!");

        // Destroy old prefabs before creating new ones
        DestroyOldPrefabs();

        if (selectedTimesTables.Count == 0)
        {
            Debug.LogWarning("No times tables selected!");
            return;
        }

        int number1;
        int number2;

        // Check if "ALL" button is active
        bool isAllSelected = selectedTimesTables.Count == 10;

        // Easy mode is ON and "ALL" is selected: Both numbers must be between 0 and 5
        if (easyModeToggle != null && easyModeToggle.isOn && isAllSelected)
        {
            number1 = Random.Range(0, 6); // Both numbers between 0-5
            number2 = Random.Range(0, 6);
        }
        // Easy mode is ON and specific tables are selected: One number must be between 0 and 5
        else if (easyModeToggle != null && easyModeToggle.isOn)
        {
            number1 = selectedTimesTables[Random.Range(0, selectedTimesTables.Count)];
            number2 = Random.Range(0, 6);  // Second number must be between 0 and 5
        }
        else
        {
            // Regular mode: Pick both numbers from the full range
            number1 = selectedTimesTables[Random.Range(0, selectedTimesTables.Count)];
            number2 = Random.Range(0, 10);
        }

        // Store the numbers for future validation
        currentEquationNumber1 = number1;
        currentEquationNumber2 = number2;

        Debug.Log("Picked numbers: " + number1 + " and " + number2);

        // For each number, randomly pick one of the three animations (0, 1, 2)
        int randomAnimIndex1 = Random.Range(0, 3);
        int randomAnimIndex2 = Random.Range(0, 3);

        // Construct prefab names
        string prefabName1 = number1.ToString();
        string prefabName2 = number2.ToString();

        if (randomAnimIndex1 > 0) prefabName1 += "_" + randomAnimIndex1;
        if (randomAnimIndex2 > 0) prefabName2 += "_" + randomAnimIndex2;

        GameObject selectedPrefab1 = FindPrefabByName(prefabName1);
        GameObject selectedPrefab2 = FindPrefabByName(prefabName2);

        if (selectedPrefab1 != null && selectedPrefab2 != null)
        {
            Debug.Log("Instantiating: " + selectedPrefab1.name + " and " + selectedPrefab2.name);

            // Increased spacing between prefabs
            float fixedSpacing = 3.5f;
            float startX = 6.0f; // Start the equation at this X position and move left

            // Instantiate the prefabs in the correct positions, using increased fixedSpacing
            instantiatedPrefabs.Add(Instantiate(selectedPrefab1, new Vector3(startX, 0, 0), Quaternion.identity));   // First number
            startX -= fixedSpacing; // Move left by fixedSpacing for the next element
            instantiatedPrefabs.Add(Instantiate(xSignPrefab, new Vector3(startX, 2.2f, 0), Quaternion.identity));       // X sign
            startX -= fixedSpacing;
            instantiatedPrefabs.Add(Instantiate(selectedPrefab2, new Vector3(startX, 0, 0), Quaternion.identity));  // Second number
            startX -= fixedSpacing;
            instantiatedPrefabs.Add(Instantiate(equalSignPrefab, new Vector3(startX, 2.2f, 0), Quaternion.identity));  // Equals sign
        }
        else
        {
            Debug.LogError("One or both prefabs are null! Check prefab names and assignments.");
        }

        // Generate the correct answer and answer options
        int correctAnswer = number1 * number2;

        if (answerManager != null)
        {
            Debug.Log("Calling GenerateAnswerOptions with correct answer: " + correctAnswer);
            GenerateAnswerOptions(correctAnswer); // Assign answers to buttons
        }
        else
        {
            Debug.LogError("AnswerManager is not assigned in the Inspector!");
        }
    }



    public void ShowCorrectAnswer(int correctAnswer)
    {
        if (correctAnswer >= 10)
        {
            int tens = correctAnswer / 10;
            int ones = correctAnswer % 10;

            InstantiateNumber(tens, new Vector3(-8.0f, 0, 0));
            InstantiateNumber(ones, new Vector3(-10.80f, 0, 0));
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
        else
        {
            Debug.LogError("Prefab for number " + number + " not found!");
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
        Debug.LogError("Prefab with name " + prefabName + " not found!");
        return null;
    }

    private void GenerateAnswerOptions(int correctAnswer)
    {
        Debug.Log("GenerateAnswerOptions called with correct answer: " + correctAnswer);

        int[] answers = new int[4];
        answers[0] = correctAnswer;

        for (int i = 1; i < 4; i++)
        {
            int wrongAnswer;
            do
            {
                wrongAnswer = Random.Range(0, 81);
            } while (wrongAnswer == correctAnswer || System.Array.Exists(answers, element => element == wrongAnswer));
            answers[i] = wrongAnswer;
        }

        Shuffle(answers);

        Debug.Log("Generated Answers: " + string.Join(", ", answers));

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
