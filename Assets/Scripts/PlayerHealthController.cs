using UnityEngine;

public class PlayerHealthController : HealthController
{
    public Texture2D boxBorder;
    private Camera mainCam;

    void Awake() {
        gameObject.tag = "Player";
    }
    
    void Start()
    {
        mainCam = gameObject.GetComponentInChildren<Camera>();
    }

    void OnGUI() {
        int cursorSize = 20;
        int cursorX = mainCam.pixelWidth / 2 - cursorSize / 4;
        int cursorY = mainCam.pixelHeight / 2 - cursorSize / 2;
        GUI.Label(new Rect(cursorX, cursorY, cursorSize, cursorSize), "+");
        int healthY = mainCam.pixelHeight - 40;
        int healthX = 30;
        int boxHeight = 20 + mainCam.pixelHeight / 70;
        float hpLengthCap = 20.0f;
        Rect healthBox;
        string healthText;
        if (healthPoints <= 0) {
            healthBox = new Rect(healthX, healthY, mainCam.pixelWidth - healthX * 2, boxHeight);
            healthText = "YOU ARE DEAD";
        } else {
            float boxWidth = ((mainCam.pixelWidth - healthX * 2) / ((float)maxHealthPoints  / (float)healthPoints)) * (maxHealthPoints / (hpLengthCap + (maxHealthPoints - hpLengthCap > 0 ? maxHealthPoints - hpLengthCap : 0)));
            healthBox = new Rect(healthX, healthY, boxWidth, boxHeight);
            healthText = $"{healthPoints.ToString()} / {maxHealthPoints.ToString()}";
        }
        GUI.DrawTexture(healthBox, Texture2D.redTexture, ScaleMode.StretchToFill, false);
        GUIStyle boxStyle = new GUIStyle();
        boxStyle.alignment = TextAnchor.MiddleCenter;
        boxStyle.fontSize = mainCam.pixelHeight / 30;
        boxStyle.normal.textColor = Color.white;
        boxStyle.border = new RectOffset(2, 2, 2, 2);
        boxStyle.normal.background = boxBorder;
        GUI.Box(healthBox, healthText, boxStyle);
    }
}
