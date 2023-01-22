using System.Collections;
using UnityEngine;

public class EnemyCombatController : CombatController
{
    public float movementSpeed = 5.0f;
    public float turningSpeed = 5.0f;
    private GameObject player;

    void Start() {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    void Update()
    {
        if (!alive || !player.GetComponent<PlayerCombatController>().alive) {
            return;
        }

        RaycastHit hit;
        Ray ray = new Ray(attackingArm.transform.position, transform.forward);
        if (Physics.SphereCast(ray, attackingArm.transform.localScale.x, out hit, weaponAttack.attackRange)) {
            if (!attacking) {
                GameObject foundObject = hit.transform.gameObject;
                GetCombatant getCombatant = foundObject.GetComponent<GetCombatant>();
                if (getCombatant != null) {
                    foundObject = getCombatant.combatant;
                }
                PlayerHealthController pController = foundObject.GetComponent<PlayerHealthController>();
                if (pController != null) {
                    StartCoroutine(attack());
                } 
            }
        } else {
            Vector3 movement = transform.InverseTransformDirection(transform.forward * movementSpeed * Time.deltaTime);
            transform.Translate(movement);
        }

        float chaseAngle = 5.0f;
        Vector3 targetDir = (player.transform.position - transform.position).normalized;
        float angleToTarget = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);
        if (Mathf.Abs(angleToTarget) > chaseAngle) {
            Vector3 targetRotation = transform.rotation.eulerAngles;
            targetRotation.y = targetRotation.y - angleToTarget;
            float calcTurnSpeed = attacking ? turningSpeed * 0.7f : turningSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * calcTurnSpeed);
        }
    }
}
