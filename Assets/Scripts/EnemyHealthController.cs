using UnityEngine.UI;

public class EnemyHealthController : HealthController
{
    public Text healthText;

    void Start() {
        healthText.text = $"{healthPoints}/{maxHealthPoints}";
    }
}
