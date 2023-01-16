using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMovementController : MonoBehaviour
{
    public float speed = 9.0f;
    public int maxStamina = 3;
    public int staminaRegenRate = 20;
    public Texture2D boxBorder;
    public Texture2D staminaTexture;
    public Canvas radialCanvas;
    private int stamina;
    private float gravity = -9.8f;
    private Vector3? dashVector = null;
    private float? staminaRegenTimer = null;
    private bool runOnce = false;
    private float stamBarWidth;
    CharacterController controller;

    void Start()
    {
        stamina = maxStamina;
        controller = gameObject.GetComponent<CharacterController>();
    }

    void OnGUI() {
        int cursorSize = 20;
        int cursorX = Camera.main.pixelWidth / 2 - cursorSize / 4;
        int cursorY = Camera.main.pixelHeight / 2 - cursorSize / 2;
        GUI.Label(new Rect(cursorX, cursorY, cursorSize, cursorSize), "+");
        int staminaY = Screen.height - 40;
        int staminaX = Screen.width / 40;
        int boxHeight = 20 + Camera.main.pixelHeight / 70;
        float staminaLengthCap = 20.0f;
        Rect staminaBox;
        string staminaText;
        float boxWidth = ((Camera.main.pixelWidth - staminaX * 2) / ((float)maxStamina  / (float)stamina)) * (maxStamina / (staminaLengthCap + (maxStamina - staminaLengthCap > 0 ? maxStamina - staminaLengthCap : 0)));
        staminaBox = new Rect(staminaX, staminaY, boxWidth, boxHeight);
        if (stamina > 0 && gameObject.GetComponent<PlayerCombatController>().alive) {
            staminaText = $"{stamina.ToString()} / {maxStamina.ToString()}";
        } else {
            staminaText = "";
        }
        
        GUI.DrawTexture(staminaBox, staminaTexture, ScaleMode.StretchToFill, false);
        GUIStyle boxStyle = new GUIStyle();
        boxStyle.alignment = TextAnchor.MiddleCenter;
        boxStyle.fontSize = Camera.main.pixelHeight / 30;
        boxStyle.normal.textColor = Color.white;
        boxStyle.border = new RectOffset(2, 2, 2, 2);
        boxStyle.normal.background = boxBorder;
        GUI.Box(staminaBox, staminaText, boxStyle);
        RectTransform[] radialRects = radialCanvas.GetComponentsInChildren<RectTransform>();
        float canvasWidth = radialCanvas.GetComponent<RectTransform>().rect.width;
        Debug.Log(Camera.main.pixelWidth);
        if (!runOnce) {
            foreach (RectTransform radialRect in radialRects) {
                radialRect.localPosition = radialRect.localPosition + new Vector3((-canvasWidth/2) + boxWidth + staminaX, 0, 0);
            }
            runOnce = true;
        } else {
            if (stamBarWidth != boxWidth) {
                foreach (RectTransform radialRect in radialRects) {
                    radialRect.localPosition = radialRect.localPosition + new Vector3(boxWidth - stamBarWidth, 0, 0);
                }
            }
        }
        stamBarWidth = boxWidth;
    }

    void Update()
    {
        if (!gameObject.GetComponent<PlayerCombatController>().alive) {
            return;
        }
        if (staminaRegenTimer == null) {
            staminaRegenTimer = staminaRegenRate;
        } else if (stamina == maxStamina) {
            staminaRegenTimer = null;
        } else if (staminaRegenTimer <= 0) {
            stamina += 1;
            staminaRegenTimer = staminaRegenRate;
        } else {
            staminaRegenTimer -= Time.deltaTime;
        }
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), gravity, Input.GetAxis("Vertical"));
        movement *= speed * Time.deltaTime;

        if (dashVector != null) {
            movement = (Vector3)dashVector;
        } else if (stamina >= 1 && Input.GetKeyDown(KeyCode.LeftShift) ) {
            stamina -= 1;
            StartCoroutine(dash(movement));
        } else {
            movement = Vector3.ClampMagnitude(movement, speed);
        }

        movement = transform.TransformDirection(movement);
        controller.Move(movement);
    }

    IEnumerator dash(Vector3 movement) {
        dashVector = movement * 5;
        yield return new WaitForSeconds(0.1f);
        dashVector = null;
    } 
}
