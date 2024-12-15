using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;          // Reference to the UI Image to fade
    public float fadeDuration = 7.0f; // Duration of the fade effect

    public void StartFade()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color endColor = new Color(0f, 0f, 0f, 1f); // Black with alpha 1

        while (timer < fadeDuration)
        {
            // Interpolate the color between start and end over time
            fadeImage.color = Color.Lerp(startColor, endColor, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        // Ensure the image is completely black at the end
        fadeImage.color = endColor;
    }
}