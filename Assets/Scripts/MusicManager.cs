using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Ensure only one instance of MusicManager exists and persists across scenes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persist across scenes
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate MusicManager instances
        }
    }

    void Start()
    {
        // Start playing music (if not already playing)
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }

        // Subscribe to scene change event
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnDestroy()
    {
        // Unsubscribe from scene change event to avoid memory leaks
        SceneManager.activeSceneChanged -= OnSceneChanged;
    }

    // Called whenever the active scene changes
    void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        // Check if we are in CompanyLogo, IntroScreen, or MainMenu
        if (newScene.name == "MainMenu" || newScene.name == "IntroScreen" || newScene.name == "CompanyLogo")
        {
            // Restart music if returning to these scenes
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            // Stop music when transitioning to a game area
            audioSource.Stop();
        }
    }
}
