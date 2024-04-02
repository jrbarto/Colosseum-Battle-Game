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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // gets component in gameobject or any of its parents
        PlayerCombatController combatController = gameObject.GetComponentInParent<PlayerCombatController>();
        if (!combatController.alive) {
            return;
        }
        float horizontalAngle = transform.localEulerAngles.y;
        if (axis == Axes.Y || axis == Axes.BOTH) {
            vertAngle -= Input.GetAxis("Mouse Y") * verticalSensitivity;
            vertAngle = Mathf.Clamp(vertAngle, minVertAngle, maxVertAngle);
        }
        if (axis == Axes.X || axis == Axes.BOTH) {
            horizontalAngle += Input.GetAxis("Mouse X") * horizontalSensitivity;
        }
        transform.localEulerAngles = new Vector3(vertAngle, horizontalAngle, 0);
    }
}
