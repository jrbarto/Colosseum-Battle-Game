using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public GameObject attackingArm;
    public GameObject weapon;
    public float attackSpeed = 5.0f;
    public int damage = 1;
    public bool alive = true;
    private bool attacking = false;    

    void Update()
    {
        if (!alive) {
            return;
        }
        if (Input.GetButtonDown("Fire1") && !attacking) {
            StartCoroutine(attack());
        }
    }

    IEnumerator attack() {
        Collider weaponCollider = weapon.GetComponent<Collider>();
        float maxArmRotation = 350.0f;
        float minArmRotation = 270.0f;
        float armRotationPerFrame = 30.0f;
        attacking = true;
        weaponCollider.enabled = true;
        while (attackingArm.transform.localEulerAngles.z < maxArmRotation) {
            float downwardRotation = armRotationPerFrame * attackSpeed * Time.deltaTime;
            attackingArm.transform.Rotate(new Vector3(0, 0, downwardRotation));
            yield return null;
        }
        weaponCollider.enabled = false;
        while (attackingArm.transform.localEulerAngles.z > minArmRotation) {
            float upwardRotation = -(armRotationPerFrame * attackSpeed * Time.deltaTime);
            attackingArm.transform.Rotate(new Vector3(0, 0, upwardRotation));
            yield return null;
        }
        attacking = false;
    }

    //TODO create an interface for player and enemy combat controllers and add this to it
    public void dropWeapon() {
        weapon.transform.parent = null;
        Collider weaponCollider = weapon.transform.GetComponent<Collider>();
        weaponCollider.enabled = true;
        weaponCollider.isTrigger = false;
        weapon.transform.GetComponent<Rigidbody>().useGravity = true;
        attackingArm.transform.Rotate(new Vector3(0, 0, 80));
        attackingArm.transform.Translate(new Vector3(0.2f, -0.3f, 0));
    }
}
