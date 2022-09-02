using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public enum Axes {
        X = 0,
        Y = 1,
        BOTH = 2
    };
    public Axes axis = Axes.X;
    public float verticalSensitivity = 9.0f;
    public float horizontalSensitivity = 9.0f;
    private float minVertAngle = -45.0f;
    private float maxVertAngle = 45.0f;
    private float vertAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalAngle = transform.localEulerAngles.y;
        if (axis == Axes.Y || axis == Axes.BOTH) {
            vertAngle -= Input.GetAxis("Mouse Y") * verticalSensitivity;
            vertAngle = Mathf.Clamp(vertAngle, minVertAngle, maxVertAngle);
        } else if (axis == Axes.X || axis == Axes.BOTH) {
            horizontalAngle += Input.GetAxis("Mouse X") * horizontalSensitivity;
        }
        transform.localEulerAngles = new Vector3(vertAngle, horizontalAngle, 0);
    }
}
