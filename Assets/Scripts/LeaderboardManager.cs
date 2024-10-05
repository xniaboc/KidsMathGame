using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    public Text[] leaderboardTextFields;  // Assign Text fields in the Inspector for displaying top 10

    private List<LeaderboardEntry> leaderboardEntries = new List<LeaderboardEntry>();

    // Struct to hold leaderboard entries
    public struct LeaderboardEntry
    {
        public string playerName;
        public int playerStreak;

        public LeaderboardEntry(string name, int streak)
        {
            playerName = name;
            playerStreak = streak;
        }
    }

    void Start()
    {
        LoadLeaderboard();
        SortAndDisplayLeaderboard();
    }

    // Load leaderboard data from PlayerPrefs
    private void LoadLeaderboard()
    {
        int count = PlayerPrefs.GetInt("LeaderboardCount", 0);

        for (int i = 0; i < count; i++)
        {
            string playerName = PlayerPrefs.GetString("PlayerName_" + i, "Unknown");
            int playerStreak = PlayerPrefs.GetInt("PlayerStreak_" + i, 0);

            LeaderboardEntry entry = new LeaderboardEntry(playerName, playerStreak);
            leaderboardEntries.Add(entry);
        }
    }

    // Sort the leaderboard by streak and display the top 10
    private void SortAndDisplayLeaderboard()
    {
        leaderboardEntries.Sort((entry1, entry2) => entry2.playerStreak.CompareTo(entry1.playerStreak)); // Sort by streak (descending)

        for (int i = 0; i < leaderboardTextFields.Length; i++)
        {
            if (i < leaderboardEntries.Count)
            {
                leaderboardTextFields[i].text = $"{leaderboardEntries[i].playerName} - Streak: {leaderboardEntries[i].playerStreak}";
            }
            else
            {
                leaderboardTextFields[i].text = "";  // Clear empty text fields
            }
        }

        // Keep only the top 10 entries
        int maxEntries = Mathf.Min(10, leaderboardEntries.Count);
        for (int i = 10; i < leaderboardEntries.Count; i++)
        {
            PlayerPrefs.DeleteKey("PlayerName_" + i);
            PlayerPrefs.DeleteKey("PlayerStreak_" + i);
        }
        PlayerPrefs.SetInt("LeaderboardCount", maxEntries);
        PlayerPrefs.Save();
    }
}
