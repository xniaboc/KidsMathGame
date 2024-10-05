using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MultiplicationChallengeManager : MonoBehaviour
{
    public GameObject[] numberPrefabs;  // Assign all 30 number prefabs in the Inspector
    public GameObject xSignPrefab;      // Assign the X sign prefab in the Inspector
    public GameObject equalSignPrefab;  // Assign the = sign prefab in the Inspector
    public ChallengeAnswerManager answerManager; // Assign the ChallengeAnswerManager in the Inspector

    public InputField playerNameInput;  // Drag the input field into this slot in the Inspector
    public Button submitButton;         // Drag the Submit button into this slot in the Inspector
    public Text timerText;              // UI Text element for the timer
    public Text streakText;             // UI Text element for the current streak
    public Text bestStreakText;         // UI Text element for the best streak
    public GameObject endGamePanel;     // UI panel to show when the game ends
    public Text endGameMessage;         // UI Text to display the game-over message
    public Button tryAgainButton;       // Assign this button in the Inspector for the "Try Again" option
    public Button mainMenuButton;       // Assign this button in the Inspector for the "Main Menu" option
    public Button startChallengeButton; // Assign this button for starting the challenge

    private List<GameObject> instantiatedPrefabs = new List<GameObject>(); // Track instantiated prefabs
    private int currentMultiplier1;     // Store the first number of the equation
    private int currentMultiplier2;     // Store the second number of the equation
    private Button correctButton;       // Store reference to the correct button
    private Color originalColor;        // Store the original button color

    private int currentStreak = 0;      // Track the current streak
    private int bestStreak = 0;         // Track the best streak
    private float timeRemaining = 1000f; // Timer for the game (16 minutes and 40 seconds)
    private bool gameIsActive = false;  // Game starts only after challenge begins

    // New list to hold all possible combinations of questions (0x0 to 9x9)
    private List<(int, int)> remainingQuestions = new List<(int, int)>();

    void Start()
    {
        LoadBestStreak();

        // Set the game inactive until the challenge is started
        gameIsActive = false;
        timerText.text = "16:40";  // Set default timer text
        streakText.text = "Streak: 0";

        // Show the Start Challenge button
        startChallengeButton.gameObject.SetActive(true);

        // Hide other game elements until the game starts
        endGamePanel.SetActive(false);  // Make sure the EndGamePanel is hidden at the start

        // Add listener for the Start Challenge button
        startChallengeButton.onClick.AddListener(StartChallenge);
    }

    void Update()
    {
        if (gameIsActive)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerUI();
            }
            else
            {
                EndGame("Time's up! Your streak: " + currentStreak);
            }
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

    public void StartChallenge()
    {
        currentStreak = 0;
        streakText.text = "Streak: 0";

        // Hide the start button and show the game
        startChallengeButton.gameObject.SetActive(false);

        // Reset timer and questions
        timeRemaining = 1000f;
        remainingQuestions.Clear();
        for (int i = 0; i <= 9; i++)
        {
            for (int j = 0; j <= 9; j++)
            {
                remainingQuestions.Add((i, j));
            }
        }

        ShuffleQuestions();  // Shuffle the questions
        gameIsActive = true; // Now the game is active
        PickRandomNumbers();
    }

    private void ShuffleQuestions()
    {
        for (int i = 0; i < remainingQuestions.Count; i++)
        {
            int randomIndex = Random.Range(0, remainingQuestions.Count);
            var temp = remainingQuestions[i];
            remainingQuestions[i] = remainingQuestions[randomIndex];
            remainingQuestions[randomIndex] = temp;
        }
    }

    public void PickRandomNumbers()
    {
        if (!gameIsActive)
        {
            Debug.LogWarning("Game is not active!");
            return;
        }

        // Destroy old prefabs before creating new ones
        DestroyOldPrefabs();

        if (remainingQuestions.Count == 0)
        {
            EndGame("Congratulations! You completed all 100 questions.");
            return;
        }

        var question = remainingQuestions[0];
        remainingQuestions.RemoveAt(0);

        currentMultiplier1 = question.Item1;
        currentMultiplier2 = question.Item2;

        // Display the question
        InstantiateEquation(currentMultiplier1, currentMultiplier2);

        int correctAnswer = currentMultiplier1 * currentMultiplier2;
        GenerateAnswerOptions(correctAnswer);
    }

    private void InstantiateEquation(int multiplier1, int multiplier2)
    {
        // Instantiate the first number at X = 7.0
        InstantiateNumber(multiplier1, new Vector3(7.0f, 0, 0));

        // Instantiate the times sign (X) at X = 3.0
        instantiatedPrefabs.Add(Instantiate(xSignPrefab, new Vector3(3.0f, 2, 0), Quaternion.identity));

        // Instantiate the second number at X = -1.0
        InstantiateNumber(multiplier2, new Vector3(-1.0f, 0, 0));

        // Instantiate the equal sign at X = -4.50
        instantiatedPrefabs.Add(Instantiate(equalSignPrefab, new Vector3(-4.50f, 2, 0), Quaternion.identity));
    }

    // Instantiate the answer based on whether it is one or two digits
    public void ShowCorrectAnswer(int correctAnswer)
    {
        if (correctAnswer >= 10)
        {
            // Two-digit answer: tens at X = -8.0, ones at X = -11.50
            int tens = correctAnswer / 10;
            int ones = correctAnswer % 10;

            InstantiateNumber(tens, new Vector3(-8.0f, 0, 0));
            InstantiateNumber(ones, new Vector3(-11.5f, 0, 0));
        }
        else
        {
            // One-digit answer at X = -8.0
            InstantiateNumber(correctAnswer, new Vector3(-8.0f, 0, 0));
        }
    }

    // Helper function to instantiate number prefabs
    private void InstantiateNumber(int number, Vector3 position)
    {
        string prefabName = number.ToString();  // Get the prefab name based on the number
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

    public void OnQuestionAnswered(bool isCorrect, Button selectedButton)
    {
        originalColor = selectedButton.GetComponent<Image>().color;

        // Disable all buttons to prevent further clicking
        DisableAnswerButtons();

        if (isCorrect)
        {
            selectedButton.GetComponent<Image>().color = Color.green;
            currentStreak++;
            streakText.text = "Streak: " + currentStreak;

            if (currentStreak > bestStreak)
            {
                bestStreak = currentStreak;
                bestStreakText.text = "Best Streak: " + bestStreak;
                SaveBestStreak();
            }

            // Show the correct answer after the player answers
            ShowCorrectAnswer(currentMultiplier1 * currentMultiplier2);

            StartCoroutine(ShowCorrectAnswerAndNextQuestion(selectedButton));
        }
        else
        {
            selectedButton.GetComponent<Image>().color = Color.red;

            // Highlight the correct answer button in green
            HighlightCorrectAnswer();

            // Show the correct answer after the wrong choice
            ShowCorrectAnswer(currentMultiplier1 * currentMultiplier2);

            EndGame("Wrong answer! Your streak: " + currentStreak);
        }
    }

    private void DisableAnswerButtons()
    {
        Button[] answerButtons = answerManager.GetAnswerButtons();
        foreach (Button button in answerButtons)
        {
            button.interactable = false;  // Disable each button
        }
    }

    private void EnableAnswerButtons()
    {
        Button[] answerButtons = answerManager.GetAnswerButtons();
        foreach (Button button in answerButtons)
        {
            button.interactable = true;  // Re-enable each button for the next question
        }
    }

    private IEnumerator ShowCorrectAnswerAndNextQuestion(Button selectedButton)
    {
        yield return new WaitForSeconds(2);
        selectedButton.GetComponent<Image>().color = originalColor;

        // Enable buttons for the next question
        EnableAnswerButtons();

        PickRandomNumbers();
    }


    private void HighlightCorrectAnswer()
    {
        Button[] answerButtons = answerManager.GetAnswerButtons();
        foreach (Button button in answerButtons)
        {
            if (button.GetComponentInChildren<Text>().text == (currentMultiplier1 * currentMultiplier2).ToString())
            {
                button.GetComponent<Image>().color = Color.green;
                break;
            }
        }
    }

    

    private void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void EndGame(string message)
    {
        // Make sure the game stops when the game ends
        gameIsActive = false;

        // Log for debugging to ensure this function is called
        Debug.Log("EndGame is being called with message: " + message);

        if (endGamePanel != null && endGameMessage != null)
        {
            // Log to check if the panel and message references are correct
            Debug.Log("EndGamePanel and EndGameMessage found.");

            // Ensure the parent objects of the EndGamePanel are active
            Transform parent = endGamePanel.transform;
            while (parent != null)
            {
                parent.gameObject.SetActive(true);
                parent = parent.parent;
            }

            // Set the panel active to show the end game screen
            endGamePanel.SetActive(true);
            endGameMessage.text = message;

            // Log for debugging to confirm that the panel is being activated
            Debug.Log("EndGamePanel should now be visible.");
        }
        else
        {
            // Log an error if the EndGamePanel or EndGameMessage is not assigned
            Debug.LogError("EndGamePanel or EndGameMessage is not assigned in the Inspector!");
        }

        // Show end game options
        ShowEndGameOptions();
    }



    private void ShowEndGameOptions()
    {
        tryAgainButton.gameObject.SetActive(true);
        mainMenuButton.gameObject.SetActive(true);

        tryAgainButton.onClick.AddListener(RestartChallenge);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    public void RestartChallenge()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void LoadBestStreak()
    {
        bestStreak = PlayerPrefs.GetInt("BestStreak", 0);
        bestStreakText.text = "Best Streak: " + bestStreak;
    }

    private void SaveBestStreak()
    {
        PlayerPrefs.SetInt("BestStreak", bestStreak);
        PlayerPrefs.Save();
    }
}
