using UnityEngine;

public class HelpChartController : MonoBehaviour
{
    public GameObject imageContainer1;  // For the first image
    public GameObject imageContainer2;  // For the second image
    public GameObject imageContainer3;  // For the third image
    public GameObject imageContainer4;  // For the fourth image
    public GameObject imageContainer5;  // For the fifth image

    // Method to show the first image and hide the others
    public void ShowImage1()
    {
        imageContainer1.SetActive(true);  // Show first image
        imageContainer2.SetActive(false); // Hide second image
        imageContainer3.SetActive(false); // Hide third image
        imageContainer4.SetActive(false); // Hide fourth image
        imageContainer5.SetActive(false); // Hide fifth image
    }

    // Method to show the second image and hide the others
    public void ShowImage2()
    {
        imageContainer1.SetActive(false); // Hide first image
        imageContainer2.SetActive(true);  // Show second image
        imageContainer3.SetActive(false); // Hide third image
        imageContainer4.SetActive(false); // Hide fourth image
        imageContainer5.SetActive(false); // Hide fifth image
    }

    // Method to show the third image and hide the others
    public void ShowImage3()
    {
        imageContainer1.SetActive(false); // Hide first image
        imageContainer2.SetActive(false); // Hide second image
        imageContainer3.SetActive(true);  // Show third image
        imageContainer4.SetActive(false); // Hide fourth image
        imageContainer5.SetActive(false); // Hide fifth image
    }

    // Method to show the fourth image and hide the others
    public void ShowImage4()
    {
        imageContainer1.SetActive(false); // Hide first image
        imageContainer2.SetActive(false); // Hide second image
        imageContainer3.SetActive(false); // Hide third image
        imageContainer4.SetActive(true);  // Show fourth image
        imageContainer5.SetActive(false); // Hide fifth image
    }

    // Method to show the fifth image and hide the others
    public void ShowImage5()
    {
        imageContainer1.SetActive(false); // Hide first image
        imageContainer2.SetActive(false); // Hide second image
        imageContainer3.SetActive(false); // Hide third image
        imageContainer4.SetActive(false); // Hide fourth image
        imageContainer5.SetActive(true);  // Show fifth image
    }

    // Method to hide all images (if needed for closing)
    public void HideImages()
    {
        imageContainer1.SetActive(false);
        imageContainer2.SetActive(false);
        imageContainer3.SetActive(false);
        imageContainer4.SetActive(false);
        imageContainer5.SetActive(false);
    }
}
