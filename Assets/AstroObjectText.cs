using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// AstroObjectText repeatedly moves a text object so that it tracks a given object on the screen
public class AstroObjectText : MonoBehaviour
{
    public Camera mainCamera;
    public Transform objectToTrack;
    public TMP_Text textUI;
    public float offset = 20;

    void Update()
    {
        // Get position of object and convert to position of object in camera POV
        Vector3 objectPosition = objectToTrack.position;
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(objectPosition);
        textUI.enabled = screenPosition.z >= 0;
        screenPosition.z = 0;
        screenPosition.y += offset;

        // Set position of text
        textUI.rectTransform.position = screenPosition;
    }
}