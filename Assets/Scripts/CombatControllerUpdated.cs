using System.Collections;
using UnityEngine;

/*
    The controller for combat related aspects of the combatant. Such as attacking,
    aspects related to handling the weapon, or the CPU chasing the player. 
*/
public class CombatControllerUpdated : MonoBehaviour
{
    public GameObject attackingArm;
    public GameObject weapon;
    public WeaponAttack weaponAttack;
    public bool alive = true;
    protected Animator animator;
    private Collider weaponCollider;

    void Awake () {
        weapon = attackingArm.transform.GetChild(0).gameObject;
        weaponCollider = weapon.transform.GetComponent<Collider>();
        weaponAttack = gameObject.GetComponentInChildren<WeaponAttack>();
        weaponAttack.attackRange = calcAttackRange();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("twoHandedWeapon", weaponAttack.twoHanded);
    }

    float calcAttackRange () {
        Collider armCollider = attackingArm.GetComponent<Collider>();
        Collider weaponCollider = weapon.GetComponent<Collider>();
        weaponCollider.enabled = true;

        Vector3 armPosition = transform.TransformPoint(attackingArm.transform.position);
        Vector3 weaponPosition = transform.TransformPoint(weapon.transform.position);
        Vector3 direction = armPosition - weaponPosition;

        Vector3 closestArmPoint = armCollider.ClosestPoint(weaponPosition);
        Vector3 farthestWeaponPoint = weaponCollider.bounds.ClosestPoint(
            armPosition + direction.normalized * weaponCollider.bounds.extents.magnitude
        );

        weaponCollider.enabled = false;
        return Vector3.Distance(closestArmPoint, farthestWeaponPoint) - 0.2f;
    }

    public void dropWeapon() {
        weapon.transform.parent = null;
        Collider weaponCollider = weapon.transform.GetComponent<Collider>();
        weaponCollider.enabled = true;
        weaponCollider.isTrigger = false;
        weapon.transform.GetComponent<Rigidbody>().useGravity = true;
    }

    protected void attack() {
        Collider weaponCollider = weapon.GetComponent<Collider>();
        weaponCollider.enabled = true;
        animator.SetBool("walking", false);
        animator.SetBool("attacking", true);
    }
}
