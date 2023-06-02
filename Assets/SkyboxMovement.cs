using UnityEngine;

public class SkyboxMovement : MonoBehaviour
{
    public float rotationSpeed = 0.3f; // Adjust this value to control the speed of the skybox movement

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed / Time.timeScale);
    }
}
