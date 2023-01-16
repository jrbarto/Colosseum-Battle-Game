using UnityEngine;

public class PlayerHealthController : HealthController
{
    public Texture2D boxBorder;

    void Awake() {
        gameObject.tag = "Player";
    }
    

    void OnGUI() {
        int cursorSize = 20;
        int cursorX = Camera.main.pixelWidth / 2 - cursorSize / 4;
        int cursorY = Camera.main.pixelHeight / 2 - cursorSize / 2;
        GUI.Label(new Rect(cursorX, cursorY, cursorSize, cursorSize), "+");
        int healthY = Camera.main.pixelHeight - 80;
        int healthX = 30;
        int boxHeight = 20 + Camera.main.pixelHeight / 70;
        float hpLengthCap = 20.0f;
        Rect healthBox;
        string healthText;
        if (healthPoints <= 0) {
            healthBox = new Rect(healthX, healthY, Camera.main.pixelWidth - healthX * 2, boxHeight);
            healthText = "YOU ARE DEAD";
        } else {
            float boxWidth = ((Camera.main.pixelWidth - healthX * 2) / ((float)maxHealthPoints  / (float)healthPoints)) * (maxHealthPoints / (hpLengthCap + (maxHealthPoints - hpLengthCap > 0 ? maxHealthPoints - hpLengthCap : 0)));
            healthBox = new Rect(healthX, healthY, boxWidth, boxHeight);
            healthText = $"{healthPoints.ToString()} / {maxHealthPoints.ToString()}";
        }
        GUI.DrawTexture(healthBox, Texture2D.redTexture, ScaleMode.StretchToFill, false);
        GUIStyle boxStyle = new GUIStyle();
        boxStyle.alignment = TextAnchor.MiddleCenter;
        boxStyle.fontSize = Camera.main.pixelHeight / 30;
        boxStyle.normal.textColor = Color.white;
        boxStyle.border = new RectOffset(2, 2, 2, 2);
        boxStyle.normal.background = boxBorder;
        GUI.Box(healthBox, healthText, boxStyle);
    }
}
