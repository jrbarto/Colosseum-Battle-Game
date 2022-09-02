using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int healthPoints = 5;
    public float secondsBetweenHit = 0.5f;

    private float lastHitTime;

    public void TakeDamage(int damage) {
        if (Time.time - lastHitTime >= secondsBetweenHit && healthPoints > 0) {
            healthPoints -= damage;
            lastHitTime = Time.time;
            Debug.Log("ENEMY HEALTH AT " + healthPoints);
        }
        if (healthPoints <= 0) {
            Die();
        }
    }

    private void Die() {
        gameObject.GetComponent<EnemyCombatController>().alive = false;
        Debug.Log("ENEMY IS DEAD!");
    }
}
