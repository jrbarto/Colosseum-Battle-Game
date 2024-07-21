using System.Collections;
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
    public WeaponAttack weaponAttack;
    public bool alive = true;
    protected Animator animator;
    private Collider weaponCollider;

    void Awake () {
        weapon = attackingArm.transform.GetChild(0).gameObject;
        weaponCollider = weapon.transform.GetComponent<Collider>();
        weaponAttack = gameObject.GetComponentInChildren<WeaponAttack>();
        animator = gameObject.GetComponent<Animator>();
        animator.SetBool("twoHandedWeapon", weaponAttack.twoHanded);
        weaponAttack.attackRange = calcAttackRange();
    }

    private float calcAttackRange () {
        List<Vector3> vertexPoints = new List<Vector3>();
        Collider weaponCollider = weapon.GetComponent<Collider>();
        weaponCollider.enabled = true;
        Collider armCollider = attackingArm.GetComponent<Collider>();
        Vector3 armPoint = armCollider.ClosestPointOnBounds(weaponCollider.transform.position);
 
        vertexPoints.Add (weaponCollider.bounds.max);
        vertexPoints.Add (weaponCollider.bounds.min);
        //vertexPoints.Add (new Vector3 (weaponCollider.bounds.max.x, weaponCollider.bounds.max.y, weaponCollider.bounds.max.z)); 
        vertexPoints.Add (new Vector3 (weaponCollider.bounds.max.x, weaponCollider.bounds.max.y, weaponCollider.bounds.min.z));    
        vertexPoints.Add (new Vector3 (weaponCollider.bounds.max.x, weaponCollider.bounds.min.y, weaponCollider.bounds.min.z));      
        vertexPoints.Add (new Vector3 (weaponCollider.bounds.max.x, weaponCollider.bounds.min.y, weaponCollider.bounds.max.z));    
        vertexPoints.Add (new Vector3 (weaponCollider.bounds.min.x, weaponCollider.bounds.min.y, weaponCollider.bounds.max.z));    
        vertexPoints.Add (new Vector3 (weaponCollider.bounds.min.x, weaponCollider.bounds.max.y, weaponCollider.bounds.max.z));
        vertexPoints.Add (new Vector3 (weaponCollider.bounds.min.x, weaponCollider.bounds.max.y, weaponCollider.bounds.min.z));
        //vertexPoints.Add (new Vector3 (weaponCollider.bounds.min.x, weaponCollider.bounds.min.y, weaponCollider.bounds.min.z)); 
 
        int maxDistanceVector = 0;
        float distance = 0;
        Vector3 getCollisionPoint = weaponCollider.ClosestPointOnBounds(armPoint);
 
        for (int i = 0; i < vertexPoints.Count; i++){
            if (i == 0) {
                distance = Vector3.Distance(getCollisionPoint, vertexPoints[i]);
                maxDistanceVector = 0;
            } else {
                float newDistance = Vector3.Distance(getCollisionPoint, vertexPoints[i]);
                if(distance < newDistance){
                    distance = newDistance;
                    maxDistanceVector = i;
                }
            }
 
        }
 
        weaponCollider.enabled = false;
        return Vector3.Distance(vertexPoints[maxDistanceVector], armPoint);
    }

    public void dropWeapon() {
        weapon.transform.parent = null;
        Collider weaponCollider = weapon.transform.GetComponent<Collider>();
        weaponCollider.enabled = true;
        weaponCollider.isTrigger = false;
        weapon.transform.GetComponent<Rigidbody>().useGravity = true;
    }

    protected void attack() {
        //Collider weaponCollider = weapon.GetComponent<Collider>();
        //weaponCollider.enabled = true;
        animator.SetBool("walking", false);
        animator.SetBool("attacking", true);
    }
}
