using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAttack : MonoBehaviour
{
    
    // TODO change enemy and player controllers to an interface to make this cleaner
    void OnTriggerEnter(Collider other) {
        GameObject foundObject = other.gameObject;
        GetEnemy getEnemy = foundObject.GetComponent<GetEnemy>();
        if (getEnemy != null) {
            foundObject = getEnemy.enemy;
        }
        EnemyController eController = foundObject.GetComponent<EnemyController>();
        if (eController != null) {
            eController.TakeDamage(1);
        }
    }
}
