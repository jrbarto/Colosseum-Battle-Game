using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthController : HealthController
{
    public Canvas uiCanvas;
    private int oldHealthPoints;
    private RectTransform healthBar;
    private GameObject healthText;
    private float maxHealthBarLength;
    private float maxHealthBarPoints = 20.0f;

    void Start() {
        gameObject.tag = "Player";
        GameObject healthBarObject = uiCanvas.transform.Find("Health Bar").gameObject;
        healthBar = healthBarObject.transform.GetComponent<RectTransform>();
        healthText = healthBarObject.transform.Find("Health Text").gameObject;
        maxHealthBarLength = healthBar.rect.width;
    }

    void Update() {
        if (healthPoints <= 0) {
            healthText.GetComponent<Text>().text = "You Are Dead!";
            healthBar.sizeDelta = new Vector2(maxHealthBarLength, healthBar.rect.height);
            healthBar.localPosition = new Vector3(-10, healthBar.localPosition.y, healthBar.localPosition.z);
        } else {
            healthText.GetComponent<Text>().text = $"{healthPoints.ToString()} / {maxHealthPoints.ToString()}";
            if (healthPoints != oldHealthPoints) {
                if (maxHealthPoints > maxHealthBarPoints) {
                    maxHealthBarPoints = maxHealthPoints;
                }
                float healthBarRatio = healthPoints / maxHealthBarPoints;
                float currentLength = healthBar.rect.width;
                healthBar.sizeDelta = new Vector2(maxHealthBarLength * healthBarRatio, healthBar.rect.height);
                healthBar.localPosition -= new Vector3((currentLength - healthBar.rect.width) / 2, 0, 0);
                oldHealthPoints = healthPoints;
            }
        }
    }
}
