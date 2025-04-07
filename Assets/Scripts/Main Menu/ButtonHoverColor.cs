using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverColor : MonoBehaviour
{
    public Button button; // Reference to the button
    public Color hoverColor; // Color to change to on hover
    private Color originalColor; // To store the original color

    void Start()
    {
        // Store the original color of the button
        originalColor = button.image.color;
    }

    // Method to change color on pointer enter
    public void OnPointerEnter()
    {
        button.image.color = hoverColor;
    }

    // Method to revert color on pointer exit
    public void OnPointerExit()
    {
        button.image.color = originalColor;
    }
}
