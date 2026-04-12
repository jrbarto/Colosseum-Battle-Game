using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public enum Axes {
        X = 0,
        Y = 1,
        BOTH = 2
    };

    public Axes axis = Axes.BOTH;

    public float verticalSensitivity = 9.0f;
    public float horizontalSensitivity = 9.0f;

    public Transform movingTransform;

    private float minVertAngle = -80.0f;
    private float maxVertAngle = 80.0f;

    private float vertAngle = 0f;
    private float yaw = 0f;

    private PauseMenu pauseMenu;
    private Quaternion pausedRotation;

    private Transform lockOnTarget;
    private CombatController lockOnTargetCombatController;
    private float lockOnRotationSpeed = 360f;

    private Camera mainCamera;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (movingTransform == null)
            movingTransform = transform;

        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<PauseMenu>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // Initialize yaw from current rotation
        yaw = movingTransform.rotation.eulerAngles.y;
    }

    void Update() {
        if (pauseMenu.paused)
            return;

        if (Input.GetKeyDown(KeyCode.Tab)) {
            if (lockOnTarget != null)
                lockOnTarget = null;
            else
                lockOnTarget = GetClosestTarget();
                if (lockOnTarget != null) {
                    lockOnTargetCombatController = lockOnTarget.GetComponent<CombatController>();
                }
        }

        if (lockOnTarget != null && lockOnTargetCombatController != null) {
            if (!lockOnTargetCombatController.alive) {
                lockOnTarget = null;
                lockOnTargetCombatController = null;
            }
        }
    }

    void LateUpdate() {
        if (pauseMenu.paused) {
            movingTransform.rotation = pausedRotation;
            return;
        }

        PlayerCombatController combatController = gameObject.GetComponentInParent<PlayerCombatController>();
        if (!combatController.alive) {
            pausedRotation = movingTransform.rotation;
            return;
        }

        // handle yaw (horizontal)
        if (axis == Axes.X || axis == Axes.BOTH) {
            if (lockOnTarget != null) {
                Vector3 targetPoint = lockOnTarget.position + Vector3.up * 1.5f;

                Vector3 flatDirection = targetPoint - movingTransform.position;
                flatDirection.y = 0f;

                if (flatDirection.sqrMagnitude > 0.001f) {
                    float targetYaw = Quaternion.LookRotation(flatDirection).eulerAngles.y;

                    float delta = Mathf.DeltaAngle(yaw, targetYaw);

                    // dampen the strength a lot when not much angle from target
                    Vector3 dir = flatDirection.normalized;
                    float angle = Vector3.Angle(movingTransform.forward, dir);
                    Debug.Log("ANGLE IS " + angle);
                    float strengthDampener = 75;

                    if (angle < 30) {
                        // smaller angle = higher dampener
                        strengthDampener += 200 - angle;
                    }
                    Debug.Log("Strength dampener is " + strengthDampener);
                    // scale correction so it's weak near center, strong when far
                    float strength = Mathf.Clamp01(Mathf.Abs(delta) / strengthDampener);

                    float adjustedSpeed = lockOnRotationSpeed * strength * 2;

                    yaw = Mathf.MoveTowardsAngle(
                        yaw,
                        targetYaw,
                        adjustedSpeed * Time.deltaTime
                    );
                }
            }
            
            yaw += Input.GetAxis("Mouse X") * horizontalSensitivity;
        }

        // handle pitch (vertical)
        if (axis == Axes.Y || axis == Axes.BOTH) {
            vertAngle -= Input.GetAxis("Mouse Y") * verticalSensitivity;
            vertAngle = Mathf.Clamp(vertAngle, minVertAngle, maxVertAngle);
        }

        // apply final rotation
        Vector3 currentEuler = movingTransform.rotation.eulerAngles;

        if (axis == Axes.X) {
            movingTransform.rotation = Quaternion.Euler(currentEuler.x, yaw, currentEuler.z);
        } else if (axis == Axes.Y) {
            movingTransform.rotation = Quaternion.Euler(-vertAngle, currentEuler.y, currentEuler.z);
        }

        pausedRotation = movingTransform.rotation;
    }

    Transform GetClosestTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject enemy in enemies) {
            Vector3 targetPoint = enemy.transform.position + Vector3.up * 1.5f;
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(targetPoint);

            if (viewportPos.z <= 0f)
                continue;

            float distance = Vector2.Distance(
                new Vector2(viewportPos.x, viewportPos.y),
                new Vector2(0.5f, 0.5f)
            );

            if (distance < closestDistance) {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        return closestEnemy;
    }
}