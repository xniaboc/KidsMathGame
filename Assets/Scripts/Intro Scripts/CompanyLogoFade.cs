using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompanyLogoFade : MonoBehaviour
{
    public CanvasGroup introCanvasGroup; // Assign the CanvasGroup component from the Canvas
    public float fadeDuration = 1f; // Duration for fade in/out
    public float displayTime = 2f; // Time to stay fully visible before fading out

    private void Start()
    {
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        // Start with fade-in
        yield return StartCoroutine(Fade(0, 1, fadeDuration));

        // Wait for the display time
        yield return new WaitForSeconds(displayTime);

        // Start fade-out
        yield return StartCoroutine(Fade(1, 0, fadeDuration));

        // After fade-out, load the main menu scene
        SceneManager.LoadScene("IntroScreen"); // Replace "MainMenuScene" with the name of your main menu scene
    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            introCanvasGroup.alpha = newAlpha;
            yield return null;
        }
        introCanvasGroup.alpha = endAlpha; // Ensure the final alpha is set
    }
}
