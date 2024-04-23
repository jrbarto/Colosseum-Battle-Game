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

        // this will not be needed when I consolidate the new combat controller for player and enemy
        CombatControllerUpdated combatController2 = gameObject.GetComponent<CombatControllerUpdated>();
        if (combatController2 != null && combatController2.alive && healthPoints <= 0) {
            Die();
        }
    }

    private void Die() {
        CombatController combatController = gameObject.GetComponent<CombatController>();
        if (combatController != null) {
            combatController.alive = false;
            combatController.dropWeapon();
        }

        // this will not be needed when I consolidate the new combat controller for player and enemy
        CombatControllerUpdated combatController2 = gameObject.GetComponent<CombatControllerUpdated>();
        if (combatController2 != null) {
            combatController2.alive = false;
            combatController2.dropWeapon();
        }
        
        if (this is PlayerHealthController) {
            StartCoroutine(Collapse());
        }
        if (this is EnemyHealthController) {
            Animator animator = gameObject.GetComponent<Animator>();
            animator.SetBool("dead", true);
            ((EnemyHealthController)this).Disappear();
        }
    }

    private IEnumerator Collapse() {
        float angle = transform.localEulerAngles.x;
        while ((angle > 180 ? angle - 360 : angle) > -90) {
            transform.Rotate(new Vector3(-2, 0, 0));
            transform.Translate(new Vector3(0, 0, -0.02f));
            angle = transform.localEulerAngles.x;
            yield return null;
        }
    }
}
