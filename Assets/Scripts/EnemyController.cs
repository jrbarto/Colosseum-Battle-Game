using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public Text healthText;
    public int healthPoints = 5;
    private int maxHealthPoints;
    public float secondsBetweenHit = 0.5f;

    private float lastHitTime;

    void Start() {
        maxHealthPoints = healthPoints;
        healthText.text = $"{healthPoints}/{maxHealthPoints}";
    }

    public void TakeDamage(int damage) {
        if (Time.time - lastHitTime >= secondsBetweenHit && healthPoints > 0) {
            healthPoints -= damage;
            lastHitTime = Time.time;
            healthText.text = $"{healthPoints}/{maxHealthPoints}";
            if (healthPoints <= 0) {
                Die();
            }
        }
    }

    private void Die() {
        EnemyCombatController combatController = transform.GetComponent<EnemyCombatController>();
        combatController.alive = false;
        combatController.dropWeapon();
        StartCoroutine(Collapse());
    }

    IEnumerator Collapse() {
        float angle = transform.localEulerAngles.x;
        while ((angle > 180 ? angle - 360 : angle) > -90) {
            transform.Rotate(new Vector3(-2, 0, 0));
            transform.Translate(new Vector3(0, 0, -0.02f));
            angle = transform.localEulerAngles.x;
            yield return null;
        }
    }
}
