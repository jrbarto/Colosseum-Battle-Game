using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthController : HealthController
{
    public Text healthText;
    private EnemySpawner enemySpawner;

    void Start() {
        gameObject.tag = "Enemy";
        healthText.text = $"{healthPoints}/{maxHealthPoints}";
        enemySpawner = GameObject.FindGameObjectsWithTag("EnemySpawner")[0].GetComponent<EnemySpawner>();
    }

    public void Disappear() {
        StartCoroutine(enemySpawner.DespawnEnemy(gameObject.transform.parent.gameObject));
    }
}
