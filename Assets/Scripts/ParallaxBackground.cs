using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float scrollSpeed = 0.5f;         // Speed of the parallax scrolling
    public GameObject[] backgrounds;         // Array to hold the background objects

    private float backgroundWidth;           // Width of the background
    private Vector3 backgroundScale = new Vector3(1.35f, 1.35f, 0f);  // Set the fixed scale
    private float backgroundZPosition = -10f;  // Set the fixed Z-position for the background
    private float backgroundYPosition = 2.85f; // Set the fixed Y-position for the background

    void Start()
    {
        // Ensure all backgrounds have the same fixed scale, Z-position, and Y-position
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].transform.localScale = backgroundScale;  // Lock the scale
            backgrounds[i].transform.position = new Vector3(backgrounds[i].transform.position.x, backgroundYPosition, backgroundZPosition); // Lock Y and Z positions
        }

        // Calculate the width of one background based on the sprite size
        backgroundWidth = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        // Scroll each background left and loop them back when they go off-screen
        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].transform.position += new Vector3(-scrollSpeed * Time.deltaTime, 0, 0); // Move only on X-axis

            // Ensure the Y and Z positions stay locked
            backgrounds[i].transform.position = new Vector3(backgrounds[i].transform.position.x, backgroundYPosition, backgroundZPosition);

            // If the background has moved off the left side, reposition it to the right
            if (backgrounds[i].transform.position.x <= -backgroundWidth)
            {
                RepositionBackground(backgrounds[i]);
            }
        }
    }

    private void RepositionBackground(GameObject background)
    {
        // Find the rightmost background and position the current background to its right
        float rightmostX = float.MinValue;
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].transform.position.x > rightmostX)
            {
                rightmostX = backgrounds[i].transform.position.x;
            }
        }

        // Move the current background to the rightmost position, plus one background width, and lock the Y and Z positions
        background.transform.position = new Vector3(rightmostX + backgroundWidth, backgroundYPosition, backgroundZPosition);
    }
}
