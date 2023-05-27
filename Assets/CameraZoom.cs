using UnityEngine;

public class CameraZoom : MonoBehaviour {
    public float zoomSpeed = 10f;
    public float zoomMin = 2.5f;
    public float zoomMax = 60f;

    public float panSpeed;

    // Since pan speed is dynamic based on zoom, changing this value will change pan speed.
    public float maxPanSpeed = 600f;
    Vector3 cameraPosition = Vector3.zero;

    private void Start() {
        cameraPosition = Camera.main.transform.position;
        setPanSpeed();
    }

    void Update() {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollWheelInput) > 0) {
            // Adjust the camera's field of view (FOV) based on scroll wheel input
            Camera.main.fieldOfView -= scrollWheelInput * zoomSpeed;

            // Limit the camera's FOV within a desired range
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, zoomMin, zoomMax);

            //Increase pan speed when zoomed out
            setPanSpeed();
        }

        // Pans the camera w/ arrow keys or WASD
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow) ||
            Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D)) {

            cameraPosition += new Vector3(Input.GetAxis("Vertical"), 0f, -Input.GetAxis("Horizontal")) * panSpeed * Time.deltaTime;
            Camera.main.transform.position = cameraPosition;
        }
    }

    public void setPanSpeed() {
        panSpeed = (Camera.main.fieldOfView / zoomMax) * maxPanSpeed;

        //panSpeed = maxPanSpeed;
    }
}