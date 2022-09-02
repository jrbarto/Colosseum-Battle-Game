using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponAttack : MonoBehaviour
{
    
    // TODO change enemy and player controllers to an interface to make this cleaner
    void OnTriggerEnter(Collider other) {
        GameObject foundObject = other.gameObject;
        GetPlayer getPlayer = foundObject.GetComponent<GetPlayer>();
        if (getPlayer != null) {
            foundObject = getPlayer.player;
        }
        PlayerController pController = foundObject.GetComponent<PlayerController>();
        if (pController != null) {
            pController.TakeDamage(1);
        }
    }
}
