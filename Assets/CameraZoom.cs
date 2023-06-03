using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class CameraZoom : MonoBehaviour
{
    public float zoomMin = 2.5f;
    public float zoomMax = 200f;

    public float panSpeed = 10f;

    // Since pan speed is dynamic based on zoom, changing this value will change pan speed.
    public float maxPanSpeed = 600f;

    // Look in all directions, pan (wasd), and move forward/backward (scroll wheel) with right click
    public Transform centerOfReference;
    public float sensitivity = 1f;
    public float scrollSpeed = 100f;
    public float cameraAcceleration = 1;

    private Vector2 rotation = new Vector2(90f, 90f);
    private bool isLooking = true;

    private KeyCode[] numKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,
                                  KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9};
    private string[] planets = { "Sun", "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune" };
    private int focused = -1;


    private void Start()
    {
        setPanSpeed();
    }

    private void Hotkeys()
    {
        for (int i = 0; i < (numKeys.Length / 2); i++)
        {
            if (Input.GetKeyDown(numKeys[i]) || Input.GetKeyDown(numKeys[i+9]))
            {
                focused = (focused != i) ? i : -1;
                break;
            }

        }
        if (focused == -1) return;
        Vector3 planetPos = GameObject.Find(planets[focused]).transform.position;
        Camera.main.transform.LookAt(planetPos);
        rotation.y = Camera.main.transform.eulerAngles.x;
        rotation.x = Camera.main.transform.eulerAngles.y;
    }

    private void Update()
    {
        // Toggles mouse look around
        if (Input.GetMouseButtonDown(1))
        {
            isLooking = !isLooking;
        }

        // Mouse look around
        if (isLooking) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (focused == -1)
            {
                rotation.x += Input.GetAxis("Mouse X") * sensitivity;
                rotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
                rotation.y = Mathf.Clamp(rotation.y, -90f, 90f);
                transform.eulerAngles = new Vector3(rotation.y, rotation.x, 0f);
            }
        
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        float deltaTime;
        if (Time.timeScale == 0) {
            deltaTime = Time.deltaTime;
        } else {
            deltaTime = (Time.deltaTime / Time.timeScale);
        }

        // Camera movement

        // XZ plane movement
        cameraAcceleration *= (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0) ? 1.001f : (1f / cameraAcceleration);
        Vector3 movementXZ =
            // Left and right (A, D) keys
            (Camera.main.transform.right * Input.GetAxis("Horizontal")
            // Up and down (W, S) keys
            + Camera.main.transform.forward * Input.GetAxis("Vertical"))
            // Acceleration
            * cameraAcceleration;

        movementXZ.y = 0f;

        // Y axis movement
        float ascendRate = 0f;
        if (Input.GetKey(KeyCode.Space)) {
            ascendRate = 0.4f; // Fly up
        } else if (Input.GetKey(KeyCode.LeftControl)) {
            ascendRate = -0.4f; // Fly down
        }

        Vector3 movementY = Vector3.up * ascendRate;
        transform.position += (movementXZ + movementY) * panSpeed * deltaTime;

        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0)
        {
            // Adjust the camera's field of view (FOV) based on scroll wheel input
            Camera.main.fieldOfView -= scrollWheelInput * 50;

            // Limit the camera's FOV within a desired range
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, zoomMin, zoomMax);

            //Increase pan speed when zoomed out
            setPanSpeed();
        }

        Hotkeys();
    }

    public void setPanSpeed()
    {
        panSpeed = (Camera.main.fieldOfView / zoomMax) * maxPanSpeed;

        //panSpeed = maxPanSpeed;
    }
}
