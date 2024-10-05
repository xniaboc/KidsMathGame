using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SubmitManager : MonoBehaviour
{
    public InputField playerNameInput;  // Reference to the player's name input field
    public Text streakText;             // Reference to the StreakText UI element
    public Button submitButton;

    private int currentStreak;

    public void SubmitPlayerName()
    {
        string playerName = playerNameInput.text;

        // Ensure the player's name is not empty or too long
        if (string.IsNullOrEmpty(playerName) || playerName.Length > 10)
        {
            Debug.LogError("Invalid player name! Ensure the name is not empty or longer than 10 characters.");
            return;
        }

        // Debug log to check what's in streakText
        Debug.Log("Content of streakText: " + streakText.text);

        // Extract the numeric part of the streakText (e.g., "1" from "Streak: 1")
        string streakValue = streakText.text.Replace("Streak: ", "").Trim();

        // Validate and parse the streakValue
        if (int.TryParse(streakValue, out currentStreak))
        {
            // Save the player name and current streak using PlayerPrefs
            SaveNewPlayerEntry(playerName, currentStreak);

            // Optionally load the leaderboard scene after submitting
            SceneManager.LoadScene("LeaderboardScene");
        }
        else
        {
            Debug.LogError("Invalid streak value! Ensure the streakText contains a valid number.");
        }
    }


    // This method will save the player's name and streak to PlayerPrefs
    private void SaveNewPlayerEntry(string playerName, int playerStreak)
    {
        int leaderboardCount = PlayerPrefs.GetInt("LeaderboardCount", 0);

        // Save the new player entry at the next available index
        PlayerPrefs.SetString("PlayerName_" + leaderboardCount, playerName);
        PlayerPrefs.SetInt("PlayerStreak_" + leaderboardCount, playerStreak);

        // Increment the leaderboard count
        leaderboardCount++;
        PlayerPrefs.SetInt("LeaderboardCount", leaderboardCount);
        PlayerPrefs.Save();
    }
}
