using UnityEngine;
using UnityEngine.AI;

public class EnemyCombatController : CombatController
{
    public float turningSpeed = 5.0f;
    public Transform enemyCenter;
    private GameObject player;
    private NavMeshAgent navAgent;
    private float currentSpeed;

    void Start() {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.stoppingDistance = weaponAttack.attackRange;
    }

    // debug function to view stopping distance when enemy is selected in scene mode during play
    // void OnDrawGizmosSelected() {
    //     navAgent = GetComponent<NavMeshAgent>();
    //     Gizmos.color = Color.red;
    //     // draw a wire sphere at the enemy's position with the radius of stoppingDistance
    //     Gizmos.DrawWireSphere(transform.position, navAgent.stoppingDistance);
    // }

    void Update()
    {
        if (!alive || !player.GetComponent<PlayerCombatController>().alive) {
            animator.SetBool("walking", false);
            animator.SetBool("attacking", false);
            return;
        }

        RaycastHit hit;
        Ray ray = new Ray(enemyCenter.position, transform.forward);
        if (Physics.SphereCast(ray, attackingArm.transform.localScale.x, out hit, weaponAttack.attackRange)) {
            GameObject foundObject = hit.transform.gameObject;
            GetCombatant getCombatant = foundObject.GetComponent<GetCombatant>();
            if (getCombatant != null) {
                foundObject = getCombatant.combatant;
            }
            PlayerHealthController pController = foundObject.GetComponent<PlayerHealthController>();
            bool attacking = animator.GetBool("attacking");
            if (pController != null && !attacking) {
                attack();
            } 
        } else {
            animator.SetBool("walking", true);
            animator.SetBool("attacking", false);
        }

        // TODO: fix this to have a running animation that plays when movespeed is high
        // TODO: also cap the animation speed so it doesn't look so dumb
        float moveSpeed = navAgent.velocity.magnitude / 3;
        Debug.Log("SETTING MOVE SPEED WITH " + Mathf.Clamp(moveSpeed, 0, 2.0f));
        animator.SetFloat("moveSpeed", Mathf.Clamp(moveSpeed, 1.0f, 2.0f));

        float targetSpeed = navAgent.desiredVelocity.magnitude;

        // Smoothly adjust the agent's speed
        if (currentSpeed < targetSpeed) {
            // Accelerate up to target speed
            currentSpeed += 2.0f * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, targetSpeed); // cap the speed at target
        } else if (currentSpeed > targetSpeed) {
            // decelerate to target speed immediately (otherwise he slides past the player)
            currentSpeed = targetSpeed; // snap to targetSpeed for instant stop
        }

        navAgent.velocity = navAgent.desiredVelocity.normalized * currentSpeed;

        float chaseAngle = 5.0f;
        Vector3 targetDir = (player.transform.position - transform.position).normalized;
        float angleToTarget = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);
        if (Mathf.Abs(angleToTarget) > chaseAngle) {
            Vector3 targetRotation = transform.rotation.eulerAngles;
            targetRotation.y = targetRotation.y - angleToTarget;
            bool attacking = animator.GetBool("attacking");
            float calcTurnSpeed = attacking ? turningSpeed * 0.1f : turningSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * calcTurnSpeed);
        }

        if (Vector3.Distance(navAgent.destination, player.transform.position) > 0.1f) {
            navAgent.SetDestination(player.transform.position);
        }
    }
}
