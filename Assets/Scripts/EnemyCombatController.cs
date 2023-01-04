using System.Collections;
using UnityEngine;

public class EnemyCombatController : CombatController
{
    public float movementSpeed = 5.0f;
    public float turningSpeed = 5.0f;
    private GameObject player;
    private bool turning = false;

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
        } else if (!turning) {
            Vector3 movement = transform.InverseTransformDirection(transform.forward * movementSpeed * Time.deltaTime);
            transform.Translate(movement);
        }

        float chaseAngle = 5.0f;
        Vector3 targetDir = (player.transform.position - transform.position).normalized;
        float angleToTarget = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);
        if (Mathf.Abs(angleToTarget) > chaseAngle && !turning) {
            StartCoroutine(turn(angleToTarget));
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
}
