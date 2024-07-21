using System.Collections;
using UnityEngine;

/*
    Controller for all aspects related to health points. This includes taking damage,
    displaying health, and dying.
*/
public class HealthController : MonoBehaviour
{
    public int healthPoints;
    public int maxHealthPoints;
    private float damageBufferTimer = 0.5f;
    private float lastHitTime;


    void Awake() {
        healthPoints = maxHealthPoints;
    }
    public void TakeDamage(int damage) {
        if (Time.time - lastHitTime >= damageBufferTimer && healthPoints > 0) {
            healthPoints -= damage;
            lastHitTime = Time.time;
        }
        CombatController combatController = gameObject.GetComponent<CombatController>();
        if (this is EnemyHealthController) {
            ((EnemyHealthController)this).healthText.text = $"{healthPoints}/{maxHealthPoints}";
        }
        if (combatController != null && combatController.alive && healthPoints <= 0) {
            Die();
        }
    }

    private void Die() {
        CombatController combatController = gameObject.GetComponent<CombatController>();
        if (combatController != null) {
            combatController.alive = false;
            combatController.dropWeapon();
        }
        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetBool("dead", true);
        if (this is EnemyHealthController) {
            ((EnemyHealthController)this).Disappear();
        }
    }
}
