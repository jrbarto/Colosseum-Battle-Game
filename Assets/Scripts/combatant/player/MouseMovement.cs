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
    public Transform movingTransform;
    private float minVertAngle = -80.0f;
    private float maxVertAngle = 80.0f;
    private float vertAngle = 0;
    private PauseMenu pauseMenu;
    private Quaternion pausedRotation; // timeScale=0 can cause Input.GetAxis to behave weird

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (movingTransform == null) {
            movingTransform = transform;
        }
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>();

    }

    void Update() {
        if (pauseMenu.paused) {
            return;
        }
        if (axis == Axes.X || axis == Axes.BOTH) {
            PlayerCombatController combatController = gameObject.GetComponentInParent<PlayerCombatController>();
            if (!combatController.alive) {
                return;
            }
            float horizontalAngle = movingTransform.eulerAngles.y;
            Vector3 currentRotation = movingTransform.eulerAngles;
            horizontalAngle += Input.GetAxis("Mouse X") * horizontalSensitivity;
            movingTransform.rotation = Quaternion.Euler(currentRotation.x, horizontalAngle, currentRotation.z);
        }
    }

    void LateUpdate()
    {
        if (pauseMenu.paused) {
            movingTransform.rotation = pausedRotation;
            return;
        }
        if (axis == Axes.Y || axis == Axes.BOTH) {
            PlayerCombatController combatController = gameObject.GetComponentInParent<PlayerCombatController>();
            if (!combatController.alive) {
                pausedRotation = movingTransform.rotation;
                return;
            }
            Vector3 currentRotation = movingTransform.eulerAngles;
            vertAngle -= Input.GetAxis("Mouse Y") * verticalSensitivity;
            vertAngle = Mathf.Clamp(vertAngle, minVertAngle, maxVertAngle);
            movingTransform.rotation = Quaternion.Euler(-vertAngle, currentRotation.y, currentRotation.z);
        }
        pausedRotation = movingTransform.rotation;
    }
}
