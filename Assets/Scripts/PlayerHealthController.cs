using UnityEngine;

public class PlayerHealthController : HealthController
{
    private Camera mainCam;

    void Start()
    {
        mainCam = gameObject.GetComponentInChildren<Camera>();
    }

    void OnGUI() {
        int cursorSize = 20;
        float cursorX = mainCam.pixelWidth / 2 - cursorSize / 4;
        float cursorY = mainCam.pixelHeight / 2 - cursorSize / 2;
        GUI.Label(new Rect(cursorX, cursorY, cursorSize, cursorSize), "+");
        float healthY = mainCam.pixelHeight - 40;
        float healthX = 30;
        Rect healthBox;
        string healthText;
        if (healthPoints <= 0) {
            healthBox = new Rect(healthX, healthY, 500, mainCam.pixelHeight / 20);
            healthText = "YOU ARE DEAD";
        } else {
            healthBox = new Rect(healthX, healthY, healthPoints * 100, mainCam.pixelHeight / 20);
            healthText = healthPoints.ToString();
        }
        GUI.DrawTexture(healthBox, Texture2D.redTexture, ScaleMode.StretchToFill, false);
        GUIStyle boxStyle = new GUIStyle();
        boxStyle.alignment = TextAnchor.MiddleCenter;
        boxStyle.normal.textColor = Color.white;
        GUI.Box(healthBox, healthText, boxStyle);
    }
}
