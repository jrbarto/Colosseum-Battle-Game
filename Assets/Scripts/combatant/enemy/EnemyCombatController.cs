using UnityEngine;
using UnityEngine.AI;

public class EnemyCombatController : CombatController
{
    public float turningSpeed = 5.0f;
    public float acceleration = 2.0f;
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

    // debug function to view spherecast when looking for player
    // void OnDrawGizmos()
    // {
    //     // Origin of the SphereCast
    //     Vector3 origin = enemyCenter.position;

    //     // Calculate the end point of the SphereCast
    //     Vector3 endPoint = origin + transform.forward * (weaponAttack.attackRange - 1);

    //     // Draw the initial sphere at the origin
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(origin, 1);

    //     // Draw a line representing the cast direction
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawLine(origin, endPoint);

    //     // Draw the sphere at the end point of the SphereCast
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawWireSphere(endPoint, 1);
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
        if (Physics.SphereCast(ray, 1, out hit, weaponAttack.attackRange - 1)) {
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

        // lowering movement speed when enemy transitions from move to attack caused 
        // them to go back to walking (from running) for a frame before attack
        if (animator.GetBool("walking")) {
            float moveSpeed = navAgent.velocity.magnitude / 3;
            animator.SetFloat("moveSpeed", Mathf.Clamp(moveSpeed, 1.0f, 2.0f));
        }

        float targetSpeed = navAgent.desiredVelocity.magnitude;

        // smoothly adjust the agent's speed
        if (currentSpeed < targetSpeed) {
            // accelerate up to target speed
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Min(currentSpeed, targetSpeed); // cap the speed at target
        } else if (currentSpeed > targetSpeed) {
            // decelerate to target speed immediately (otherwise he slides past the player)
            currentSpeed = targetSpeed; // snap to targetSpeed for instant stop
        }

        navAgent.velocity = navAgent.desiredVelocity.normalized * currentSpeed;

        float chaseAngle = 1.0f;
        Vector3 targetDir = (player.transform.position - transform.position).normalized;
        float angleToTarget = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);
        if (Mathf.Abs(angleToTarget) > chaseAngle) {
            Vector3 targetRotation = transform.rotation.eulerAngles;
            targetRotation.y = targetRotation.y - angleToTarget;
            bool attacking = animator.GetBool("attacking");
            float calcTurnSpeed = attacking ? turningSpeed * 0.5f : turningSpeed;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), Time.deltaTime * calcTurnSpeed);
        }

        if (Vector3.Distance(navAgent.destination, player.transform.position) > 0.1f) {
            navAgent.SetDestination(player.transform.position);
        }
    }
}
