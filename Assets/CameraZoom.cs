using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 10f;

    void Update()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollWheelInput) > 0)
        {
            // Adjust the camera's field of view (FOV) based on scroll wheel input
            Camera.main.fieldOfView += (scrollWheelInput * zoomSpeed)*-1;

            // Limit the camera's FOV within a desired range
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 2.5f, 60f);
        }
    }
}
