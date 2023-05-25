using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    public float rotationSpeed = 0.005f;

    void Update()
    {
        // Calculate the rotation amount based on time and speed
        float rotationAmount = Time.deltaTime * rotationSpeed;

        // Apply the rotation around the x-axis
/*        transform.Rotate(Vector3.forward, rotationAmount);
*/    }
}
