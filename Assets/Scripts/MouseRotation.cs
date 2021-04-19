using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotation : MonoBehaviour
{
    public float mouseSensitivity;

    public Transform playerBody;
    public Transform Camera;

    float xRotation = 0f;
    float yRotation = 0f;
    
    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -30, 90);
        yRotation += mouseX;

        playerBody.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        Camera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}


