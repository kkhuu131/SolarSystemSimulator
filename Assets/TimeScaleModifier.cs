using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeScaleModifier : MonoBehaviour
{
    public Slider timeScaleSlider;
    public TextMeshProUGUI timeScaleText;

    private void Start()
    {
        // Initialize the slider value and text
        timeScaleSlider.value = Time.timeScale;
        UpdateTimeScaleText();
    }

    public void ChangeTimeScale()
    {
        // Called when the slider value changes
        Time.timeScale = timeScaleSlider.value;

        // Update the text
        UpdateTimeScaleText();
    }

    private void UpdateTimeScaleText()
    {
        // Update the text with the current time scale value
        timeScaleText.text = "Time Scale: " + Time.timeScale.ToString("F1");
    }
}