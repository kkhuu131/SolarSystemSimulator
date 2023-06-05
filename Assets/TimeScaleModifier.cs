using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEngine.Rendering;

public class TimeScaleModifier : MonoBehaviour
{
    public UnityEngine.UI.Slider timeScaleSlider, massSlider, volumeSlider;
    public TextMeshProUGUI timeScaleText, massText, volumeText;

    private string[] planets = { "Sun", "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune" };
    private float[] masses = { 333000, 0.0553f, 0.815f, 1, 0.107f, 318, 95.2f, 14.5f, 17.1f };
    private float[] volumes = { 50f, 1.63f, 4.03f, 4.25f, 2.26f, 46.6f, 38.8f, 16.9f, 16.4f };
    private int[] massMultipliers = { 7, 7, 7, 7, 7, 7, 7, 7, 7 };
    private int[] volumeMultipliers = { 9, 9, 9, 9, 9, 9, 9, 9, 9 };
    private float[] positions = { 0f, 57.9f, 108.2f, 149.6f, 227.9f, 778.5f, 1000.43f, 2000.87f, 4000.5f };

    private int focused = -1;
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

    public void ChangeMass()
    {
        // Called when the slider value changes
        massMultipliers[focused] = (int)massSlider.value;

        // Update object mass
        float newMassMultiplier = .0000001f * Mathf.Pow(10, massMultipliers[focused]);
        GameObject.Find(planets[focused]).GetComponent<Rigidbody>().mass = masses[focused] * newMassMultiplier;

        // Update the text
        UpdateMassText(newMassMultiplier);
    }

    private void UpdateMassText(float mass)
    {
        // Update the text with the current mass multiplier value
        if (massMultipliers[focused] < 7)
            massText.text = "Mass: " + (mass).ToString("F" + (7 - massMultipliers[focused]).ToString()) + "x";
        else
            massText.text = "Mass: " + (mass).ToString("F0") + "x";
    }

    public void ChangeVolume()
    {
        // Called when the slider value changes
        volumeMultipliers[focused] = (int)volumeSlider.value;

        // Update object volume
        // .1 .2 .3 .4 .5 .6 .7 .8 .9 1 2 3 4 5 6 7 8 9 10
        float newVolumeMultiplier = (volumeMultipliers[focused] - 9f) + 1;
        if (newVolumeMultiplier < 1) newVolumeMultiplier = 1 + (0.1f * (newVolumeMultiplier - 1));
        float newVolume = volumes[focused] * newVolumeMultiplier;
        GameObject.Find(planets[focused]).GetComponent<Transform>().localScale = new Vector3(newVolume, newVolume, newVolume);

        // Update the text
        UpdateVolumeText(newVolumeMultiplier);
    }

    private void UpdateVolumeText(float volume)
    {
        // Update the text with the current volume multiplier value
        volumeText.text = "Volume: " + (volume).ToString("F1") + "x";
    }
    public void setFocused(int focus)
    {
        focused = focus;
        if (focused != -1)
        {
            massSlider.value = massMultipliers[focus];
            volumeSlider.value = volumeMultipliers[focus];
            ChangeMass();
            ChangeVolume();
        }
    }
    
    private void changePosition()
    {
        GameObject planet = GameObject.Find(planets[focused]);
        planet.GetComponent<Transform>().position = new Vector3(positions[focused], 0, 0);
        planet.GetComponent<Rigidbody>().velocity = Vector3.zero;
        planet.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        if (focused != 0) planet.GetComponent<TrailRenderer>().Clear();
    }

    public void reset_all()
    {
        massSlider.value = 7;
        volumeSlider.value = 9;
        for (int i = 0; i < 9; i++)
        {
            focused = i;
            ChangeMass();
            ChangeVolume();
            changePosition();
        }
        GameObject.Find("PhysicsSimulation").GetComponent<PhysicsSimulation>().InitialVelocity();
        timeScaleSlider.value = 1;
        ChangeTimeScale();
        focused = -1;
    }
}