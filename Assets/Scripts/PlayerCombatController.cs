using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    public GameObject attackingArm;
    public float attackSpeed = 5.0f;
    public float attackDistance = 5.0f;
    public int damage = 1;
    private bool attacking = false;    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !attacking) {
            StartCoroutine(attack());
        }
    }

    IEnumerator attack() {
        float maxArmRotation = 350.0f;
        float minArmRotation = 270.0f;
        float armRotationPerFrame = 50.0f;
        attacking = true;
        while (attackingArm.transform.localEulerAngles.z < maxArmRotation) {
            float downwardRotation = armRotationPerFrame * attackSpeed * Time.deltaTime;
            attackingArm.transform.Rotate(new Vector3(0, 0, downwardRotation));
            yield return null;
        }
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.SphereCast(ray, transform.localScale.x, out hit, attackDistance)) {
            GameObject foundObject = hit.transform.gameObject;
            GetEnemy getEnemy = foundObject.GetComponent<GetEnemy>();
            if (getEnemy != null) {
                foundObject = getEnemy.enemy;
            }
            EnemyController enemyController = foundObject.GetComponent<EnemyController>();
            if (enemyController != null) {
                enemyController.TakeDamage(damage);
            }
        }
        while (attackingArm.transform.localEulerAngles.z > minArmRotation) {
            float upwardRotation = -(armRotationPerFrame * attackSpeed * Time.deltaTime);
            attackingArm.transform.Rotate(new Vector3(0, 0, upwardRotation));
            yield return null;
        }
        attacking = false;
    }
}
