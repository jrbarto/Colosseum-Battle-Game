using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombatController : MonoBehaviour
{
    public GameObject player;
    public GameObject attackingArm;
    public GameObject weapon;
    public float attackRange = 0.3f;
    public float attackSpeed = 5.0f;
    public float movementSpeed = 5.0f;
    public float turningSpeed = 5.0f;
    public int damage = 1;
    private bool armSwinging = false;
    private bool turning = false;

    // Update is called once per frame
    void Update()
    {
        float chaseAngle = 10.0f;
        Vector3 targetDir = (player.transform.position - transform.position).normalized;
        float angleToTarget = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);
        if (Mathf.Abs(angleToTarget) > chaseAngle && !turning) {
            StartCoroutine(turn(angleToTarget));
        }
        
        // PlayerController pController;
        // if (SphereCastForPlayer(
        //     out pController,
        //     transform.position,
        //     transform.forward,
        //     transform.localScale.x,
        //     chaseDistance
        // )) {
        //     if (!armSwinging && pController != null) {
        //         StartCoroutine(attackPlayer(pController));
        //     }
        // }  else if (!turning) {
        //     Vector3 movement = transform.InverseTransformDirection(transform.forward * movementSpeed * Time.deltaTime);
        //     transform.Translate(movement);
        // }

        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.SphereCast(ray, transform.localScale.x, out hit, attackRange)) {
            if (!armSwinging) {
                GameObject foundObject = hit.transform.gameObject;
                GetPlayer getPlayer = foundObject.GetComponent<GetPlayer>();
                if (getPlayer != null) {
                    foundObject = getPlayer.player;
                }
                PlayerController pController = foundObject.GetComponent<PlayerController>();
                if (pController != null) {
                    StartCoroutine(attackPlayer(pController));
                } 
            }
        } else if (!turning) {
            Vector3 movement = transform.InverseTransformDirection(transform.forward * movementSpeed * Time.deltaTime);
            transform.Translate(movement);
        }
    }

    IEnumerator turn(float angleToTarget) {
        float rotationPerFrame = (angleToTarget < 0 ? 50.0f : -50.0f) * turningSpeed;
        float rotated = 0.0f;
        turning = true;
        while (Mathf.Abs(rotated) < Mathf.Abs(angleToTarget)) {
            transform.Rotate(0, rotationPerFrame * Time.deltaTime, 0);
            rotated += rotationPerFrame;
            yield return null;
        }
        turning = false;
    }

    IEnumerator attackPlayer(PlayerController pController) {
        Collider weaponCollider = weapon.GetComponent<Collider>();
        float maxArmRotation = 350.0f;
        float minArmRotation = 270.0f;
        float armRotationPerFrame = 50.0f;
        armSwinging = true;
        weaponCollider.enabled = true;
        while (attackingArm.transform.localEulerAngles.z < maxArmRotation) {
            float downwardRotation = armRotationPerFrame * attackSpeed * Time.deltaTime;
            attackingArm.transform.Rotate(new Vector3(0, 0, downwardRotation));
            yield return null;
        }
        weaponCollider.enabled = false;
        // if (Vector3.Distance(attackingArm.transform.position, pController.transform.position) <= forwardAttackRange) {
        //     pController.TakeDamage(damage);
        // }
        while (attackingArm.transform.localEulerAngles.z > minArmRotation) {
            float upwardRotation = -(armRotationPerFrame * attackSpeed * Time.deltaTime);
            attackingArm.transform.Rotate(new Vector3(0, 0, upwardRotation));
            yield return null;
        }
        armSwinging = false;
    }

    // bool SphereCastForPlayer(
    //     out PlayerController pController,
    //     Vector3 position, 
    //     Vector3 direction, 
    //     float radius, 
    //     float distance
    // ) {
    //     pController = null;
    //     RaycastHit hit;
    //     Ray ray = new Ray(position, direction);
    //     if (Physics.SphereCast(ray, radius, out hit, distance)) {
    //         GameObject foundObject = hit.transform.gameObject;
    //         GetPlayer getPlayer = foundObject.GetComponent<GetPlayer>();
    //         if (getPlayer != null) {
    //             foundObject = getPlayer.player;
    //         }
    //         pController = foundObject.GetComponent<PlayerController>();
    //         return true;
    //     }
    //     return false;
    // }
}
