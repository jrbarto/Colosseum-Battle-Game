using System.Collections;
using UnityEngine;

public abstract class HealthController : MonoBehaviour
{
    public int healthPoints;
    public int maxHealthPoints;
    protected float secondsBetweenHit = 0.5f;
    protected float lastHitTime;

    public void TakeDamage(int damage) {
        if (Time.time - lastHitTime >= secondsBetweenHit && healthPoints > 0) {
            healthPoints -= damage;
            lastHitTime = Time.time;
        }
        CombatController combatController = gameObject.GetComponent<CombatController>();
        if (this is EnemyHealthController) {
            ((EnemyHealthController)this).healthText.text = $"{healthPoints}/{maxHealthPoints}";
        }
        if (combatController.alive && healthPoints <= 0) {
            Die();
        }
    }

    private void Die() {
        CombatController combatController = gameObject.GetComponent<CombatController>();
        combatController.alive = false;
        combatController.dropWeapon();
        StartCoroutine(Collapse());
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
