using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 10f;
    public float zoomMin = 2.5f;
    public float zoomMax = 200f;

    public float panSpeed = 10f;

    // Since pan speed is dynamic based on zoom, changing this value will change pan speed.
    public float maxPanSpeed = 600f;

    // Look in all directions, pan (wasd), and move forward/backward (scroll wheel) with right click
    public Transform centerOfReference;
    public float sensitivity = 1f;
    public float scrollSpeed = 50f;

    private Vector2 rotation = new Vector2(90f, 90f);
    private bool isLooking = false;

    private KeyCode[] numKeys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9,
                                  KeyCode.Keypad1, KeyCode.Keypad2, KeyCode.Keypad3, KeyCode.Keypad4, KeyCode.Keypad5, KeyCode.Keypad6, KeyCode.Keypad7, KeyCode.Keypad8, KeyCode.Keypad9};
    private string[] planets = { "Sun", "Mercury", "Venus", "Earth", "Mars", "Jupiter", "Saturn", "Uranus", "Neptune" };
    private int index = -1;


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
                index = (index != i) ? i : -1;
                break;
            }

        }
        if (index == -1) return;
        Vector3 planetPos = GameObject.Find(planets[index]).transform.position;
        Camera.main.transform.LookAt(planetPos);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            isLooking = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (Input.GetMouseButtonUp(1))
        {
            isLooking = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (isLooking)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            rotation.x += mouseX;
            rotation.y -= mouseY;
            rotation.y = Mathf.Clamp(rotation.y, -90f, 90f);

            transform.eulerAngles = new Vector3(rotation.y, rotation.x, 0f);

            // Calculate the panning direction based on the camera's forward vector
            Vector3 panDirection = Quaternion.Euler(0f, rotation.x, 0f) * Vector3.right;

            // Apply panning based on input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            float deltaTime;
            if (Time.timeScale == 0)
            {
                deltaTime = Time.deltaTime;
            }
            else {
                deltaTime = (Time.deltaTime / Time.timeScale);
            }
            Vector3 pan = (panDirection * horizontal + transform.up * vertical) * panSpeed * deltaTime;
            transform.position += pan;

            // Apply forward/backward movement based on scroll wheel input 
            // TODO
            /*          float scroll = Input.GetAxis("Mouse ScrollWheel");
                        Vector3 moveDirection = transform.forward * scroll * scrollSpeed * Time.deltaTime;
                        transform.position += moveDirection;*/
        }

        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scrollWheelInput) > 0)
        {
            // Adjust the camera's field of view (FOV) based on scroll wheel input
            Camera.main.fieldOfView -= scrollWheelInput * zoomSpeed;

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
