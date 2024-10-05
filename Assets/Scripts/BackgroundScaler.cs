using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        mainCamera = Camera.main; // Get the main camera
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component attached to this object

        // Scale the background to fit the screen size
        ScaleBackgroundToFitScreen();
    }

    void ScaleBackgroundToFitScreen()
    {
        // Get the height and width of the camera in world units
        float worldScreenHeight = 2f * mainCamera.orthographicSize;
        float worldScreenWidth = worldScreenHeight * mainCamera.aspect;

        // Get the sprite size in world units
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // Calculate the scale factors for width and height
        float scaleFactorX = worldScreenWidth / spriteSize.x;
        float scaleFactorY = worldScreenHeight / spriteSize.y;

        // Apply the largest scale factor to preserve the aspect ratio of the background
        Vector3 scale = new Vector3(scaleFactorX, scaleFactorY, 1);
        transform.localScale = scale;

        // Optionally log to check values
        Debug.Log("Screen Width (World Units): " + worldScreenWidth + ", Screen Height: " + worldScreenHeight);
        Debug.Log("Sprite Width: " + spriteSize.x + ", Sprite Height: " + spriteSize.y);
        Debug.Log("Scale Applied: " + scale);
    }
}
