using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerMovementController : MonoBehaviour
{
    public float speed = 9.0f;
    public int maxStaminaPoints = 3;
    public int staminaRegenRate = 20;
    public Canvas uiCanvas;
    private int staminaPoints;
    private float gravity = -9.8f;
    private Vector3? dashVector = null;
    private float? staminaRegenTimer = null;
    private bool runOnce = false;
    private CharacterController controller;
    private int oldStaminaPoints;
    private RectTransform staminaBar;
    private Transform staminaRadialTransform;
    private GameObject[] staminaRadialSegments;
    private GameObject staminaText;
    private float maxStaminaBarLength;
    private float maxStaminaBarPoints = 20;

    void Start()
    {
        staminaPoints = maxStaminaPoints;
        controller = gameObject.GetComponent<CharacterController>();
        Transform staminaBarTransform = uiCanvas.transform.Find("Stamina Bar");
        staminaBar = staminaBarTransform.GetComponent<RectTransform>();
        staminaText = staminaBarTransform.Find("Stamina Text").gameObject;
        maxStaminaBarLength = staminaBar.rect.width;
        staminaRadialTransform = uiCanvas.transform.Find("Stamina Radial");
        List<GameObject> transitiveSegments = new List<GameObject>();
        for (int i = 0; i < staminaRadialTransform.childCount; i++) {
            transitiveSegments.Add(staminaRadialTransform.GetChild(i).gameObject);
        }
        staminaRadialSegments = transitiveSegments.ToArray();
    }

    void Update()
    {
        bool alive = gameObject.GetComponent<PlayerCombatController>().alive;
        if (staminaPoints == 0 || !alive) {
            staminaPoints = 0;
            staminaText.GetComponent<Text>().text = "";
        } else {
            staminaText.GetComponent<Text>().text = $"{staminaPoints.ToString()} / {maxStaminaPoints.ToString()}";
        }
        if (staminaPoints != oldStaminaPoints) {
            if (maxStaminaPoints > maxStaminaBarPoints) {
                maxStaminaBarPoints = maxStaminaPoints;
            }
            float staminaBarRatio = staminaPoints / maxStaminaBarPoints;
            float currentLength = staminaBar.rect.width;
            staminaBar.sizeDelta = new Vector2(maxStaminaBarLength * staminaBarRatio, staminaBar.rect.height);
            staminaBar.localPosition -= new Vector3((currentLength - staminaBar.rect.width) / 2, 0, 0);
            staminaRadialTransform.localPosition = new Vector3(
                staminaBar.localPosition.x + (staminaBar.sizeDelta.x / 2) + 10, 
                staminaRadialTransform.localPosition.y,
                staminaRadialTransform.localPosition.z
            );
            oldStaminaPoints = staminaPoints;
        }
        rotateRadial(alive);
        if (!alive) {
            return;
        }
        if (staminaRegenTimer == null) {
            staminaRegenTimer = staminaRegenRate;
        } else if (staminaPoints == maxStaminaPoints) {
            staminaRegenTimer = null;
        } else if (staminaRegenTimer <= 0) {
            staminaPoints += 1;
            staminaRegenTimer = staminaRegenRate;
        } else {
            staminaRegenTimer -= Time.deltaTime;
        }
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), gravity, Input.GetAxis("Vertical"));
        movement *= speed * Time.deltaTime;

        if (dashVector != null) {
            movement = (Vector3)dashVector;
        } else if (
            staminaPoints >= 1 && 
            Input.GetKeyDown(KeyCode.LeftShift) &&
            (movement.x != 0 || movement.z != 0)
        ) {
            staminaPoints -= 1;
            StartCoroutine(dash(movement));
        } else {
            movement = Vector3.ClampMagnitude(movement, speed);
        }

        movement = transform.TransformDirection(movement);
        controller.Move(movement);
    }

    private void rotateRadial(bool alive) {
        if (staminaRegenTimer != null && alive) {
            float fillRatio = (float)(staminaRegenRate - staminaRegenTimer) / staminaRegenRate;
            for (int i = 0; i < Mathf.Floor(staminaRadialSegments.Length * fillRatio); i++) {
                GameObject segment = staminaRadialSegments[i];
                if (fillRatio >= 1) {
                    segment.SetActive(false);
                } else {
                    segment.SetActive(true);
                }
            }
        } else {
            for (int i = 0; i < staminaRadialSegments.Length; i++) {
                GameObject segment = staminaRadialSegments[i];
                segment.SetActive(false);
            }
        }

    }

    IEnumerator dash(Vector3 movement) {
        dashVector = movement * 5;
        yield return new WaitForSeconds(0.1f);
        dashVector = null;
    } 
}
