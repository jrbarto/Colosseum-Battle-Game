using System.Collections.Generic;
using UnityEngine;

/*
    The controller for combat related aspects of the combatant. Such as attacking,
    aspects related to handling the weapon, or the CPU chasing the player. 
*/
public class CombatController : MonoBehaviour
{
    public GameObject attackingArm;
    public GameObject weapon;
    protected WeaponAttack weaponAttack;
    public bool alive = true;
    protected Animator animator;
    private Collider weaponCollider;

    void Awake () {
        weapon = attackingArm.transform.GetChild(0).gameObject;
        weaponCollider = weapon.transform.GetComponent<Collider>();
        weaponAttack = gameObject.GetComponentInChildren<WeaponAttack>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("twoHandedWeapon", weaponAttack.twoHanded);
    }

    public bool isAttacking() {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        return stateInfo.IsTag("attacking");
    }

    public void dropWeapon() {
        weapon.transform.parent = null;
        Collider weaponCollider = weapon.transform.GetComponent<Collider>();
        weaponCollider.enabled = true;
        weaponCollider.isTrigger = false;
        weapon.transform.GetComponent<Rigidbody>().useGravity = true;
    }

    protected void attack() {
        if (this is EnemyCombatController) {
            animator.SetBool("walking", false);
            animator.SetBool("attacking", true);
        } else {
            if (weaponAttack.twoHanded) {
                animator.CrossFade("Base Layer.Twohand Attacking", 0.1f);
            } else {
                animator.CrossFade("Base Layer.Onehand Attacking", 0.1f);
            }
        }
    }
}
