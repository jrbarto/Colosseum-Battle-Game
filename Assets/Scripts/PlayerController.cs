using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera camera;
    public int healthPoints;
    public float secondsBetweenHit = 0.5f;
    private float lastHitTime;

    // Start is called before the first frame update
    void Start()
    {
        healthPoints = 5;
    }

    void OnGUI() {
        int cursorSize = 20;
        float cursorX = camera.pixelWidth / 2 - cursorSize / 4;
        float cursorY = camera.pixelHeight / 2 - cursorSize / 2;
        GUI.Label(new Rect(cursorX, cursorY, cursorSize, cursorSize), "+");
        float healthY = camera.pixelHeight - 40;
        float healthX = 30;
        Rect healthBox;
        string healthText;
        if (healthPoints <= 0) {
            healthBox = new Rect(healthX, healthY, 500, camera.pixelHeight / 20);
            healthText = "YOU ARE DEAD";
        } else {
            healthBox = new Rect(healthX, healthY, healthPoints * 100, camera.pixelHeight / 20);
            healthText = healthPoints.ToString();
        }
        GUI.backgroundColor = Color.red;
        GUI.Box(healthBox, healthText);
    }

    public void TakeDamage(int damage) {
        if (Time.time - lastHitTime >= secondsBetweenHit && healthPoints > 0) {
            healthPoints -= damage;
            lastHitTime = Time.time;
        }
        if (healthPoints <= 0) {
            Die();
        }
    }

    private void Die() {
        gameObject.GetComponent<PlayerCombatController>().alive = false;
        gameObject.GetComponent<Movement>().alive = false;
        Debug.Log("YOU DIED!!");
    }
}
