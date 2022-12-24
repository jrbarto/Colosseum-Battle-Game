using UnityEngine.UI;

public class EnemyHealthController : HealthController
{
    public Text healthText;

    void Awake() {
        gameObject.tag = "Enemy";
    }

    void Start() {
        healthText.text = $"{healthPoints}/{maxHealthPoints}";
    }
}
