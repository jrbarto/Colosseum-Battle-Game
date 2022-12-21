using System.Collections;
using UnityEngine;

/*
    The controller for combat related aspects of the combatant. Such as attacking,
    aspects related to handling the weapon, or the CPU chasing the player. 
*/
public class CombatController : MonoBehaviour
{
    public GameObject attackingArm;
    public GameObject weapon;
    public WeaponAttack weaponAttack;
    public bool alive = true;
    protected bool attacking = false;
    private Collider weaponCollider;

    void Awake () {
        weaponCollider = weapon.transform.GetComponent<Collider>();
        weaponAttack = gameObject.GetComponentInChildren<WeaponAttack>();
        weaponAttack.attackRange = calcAttackRange();
    }

    float calcAttackRange () {
        float highestArmPoint = attackingArm.GetComponent<Collider>().bounds.max.y;
        Collider weaponCollider = weapon.GetComponent<Collider>();
        weaponCollider.enabled = true;
        float highestWeaponPoint = weaponCollider.bounds.max.y;
        weaponCollider.enabled = false;
        return highestWeaponPoint - highestArmPoint;
    }

    public void dropWeapon() {
        weapon.transform.parent = null;
        Collider weaponCollider = weapon.transform.GetComponent<Collider>();
        weaponCollider.enabled = true;
        weaponCollider.isTrigger = false;
        weapon.transform.GetComponent<Rigidbody>().useGravity = true;
        attackingArm.transform.Rotate(new Vector3(0, 0, 80));
        attackingArm.transform.Translate(new Vector3(0.2f, -0.3f, 0));
    }

    protected IEnumerator attack() {
        Collider weaponCollider = weapon.GetComponent<Collider>();
        float maxArmRotation = 350.0f;
        float minArmRotation = 270.0f;
        float armRotationPerFrame = 50.0f;
        attacking = true;
        weaponCollider.enabled = true;
        while (attackingArm.transform.localEulerAngles.z < maxArmRotation) {
            float downwardRotation = armRotationPerFrame * weaponAttack.attackSpeed * Time.deltaTime;
            attackingArm.transform.Rotate(new Vector3(0, 0, downwardRotation));
            yield return null;
        }
        weaponCollider.enabled = !alive;
        while (attackingArm.transform.localEulerAngles.z > minArmRotation) {
            if (!alive) {
                yield break;
            }
            float upwardRotation = -(armRotationPerFrame * weaponAttack.attackSpeed * Time.deltaTime);
            attackingArm.transform.Rotate(new Vector3(0, 0, upwardRotation));
            yield return null;
        }
        attacking = false;
    }
}
