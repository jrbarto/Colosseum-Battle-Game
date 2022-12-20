using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public float attackRange = 0.7f;
    public float attackSpeed = 2.0f;
    public int damage = 1;
    private GameObject selfCombatant;

    void Start() {
        selfCombatant = gameObject.GetComponentInParent<HealthController>().gameObject;
    }
    
    void OnTriggerEnter(Collider other) {
        GameObject foundObject = other.gameObject;
        GetCombatant getCombatant = foundObject.GetComponent<GetCombatant>();
        if (getCombatant != null) {
            foundObject = getCombatant.combatant;
        }
        if (foundObject == selfCombatant) {
            return;
        }
        HealthController healthController = foundObject.GetComponent<HealthController>();
        if (healthController != null) {
            healthController.TakeDamage(damage);
        }
    }
}
