using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadMainMenu()
    {
        // Fade to the Main Menu scene
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadMultiplication()
    {
        // Fade to the Multiplication scene
        SceneManager.LoadScene("MultiplicationScene");
    }

    public void LoadSquareRoot()
    {
        // Fade to the Square Root scene
        SceneManager.LoadScene("SquareRootScene");
    }

    public void LoadMultiplicationChallenge()
    {
        // Fade to the Multiplication Challenge scene
        SceneManager.LoadScene("MultiplicationChallengeScene");
    }

    public void LoadHelperChartScene()
    {
        // Fade to the Helper Chart scene
        SceneManager.LoadScene("HelperChartScene");
    }

    public void LoadChallengeLeaderboard()
    {
        // Fade to the Challenge Leaderboard scene
        SceneManager.LoadScene("LeaderboardScene");
    }

    public void LoadDivision()
    {
        // Fade to the Division scene
        SceneManager.LoadScene("DivisionScene");
    }

    public void LoadPattern()
    {
        // Fade to the Pattern scene
        SceneManager.LoadScene("PatternScene");
    }

    public void LoadAddition()
    {
        // Fade to the Addition scene
        SceneManager.LoadScene("AdditionScene");
    }

    public void LoadSubtraction()
    {
        // Fade to the Subtraction scene
        SceneManager.LoadScene("SubtractionScene");
    }
}
