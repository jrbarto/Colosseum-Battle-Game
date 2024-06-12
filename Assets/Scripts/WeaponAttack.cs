using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
    public float attackRange = 0.7f;
    public float attackSpeed = 2.0f;
    public int damage = 1;
    public bool twoHanded = false;
    private GameObject selfCombatant;
    private Vector3 previousPosition;
    private Vector3 previousRootPosition;
    private CombatControllerUpdated combatController;
    private float yVelocity;

    void Start() {
        selfCombatant = gameObject.GetComponentInParent<HealthController>().gameObject;
        previousPosition = gameObject.transform.position;
        previousRootPosition = transform.parent.root.transform.position;
        combatController = transform.GetComponentInParent<CombatControllerUpdated>();
    }

    void FixedUpdate() {
        if (combatController && combatController.alive) {
            Vector3 velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
            previousPosition = transform.position;
            Vector3 rootPosition = transform.parent.root.position;
            Vector3 rootVelocity = (rootPosition - previousRootPosition) / Time.fixedDeltaTime;
            previousRootPosition = rootPosition;
            yVelocity = velocity.y - rootVelocity.y;
            Collider weaponCollider = GetComponent<Collider>();
            if (yVelocity < -1) {
                weaponCollider.enabled = true;
            } else {
                weaponCollider.enabled = false;
            }
        }
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
