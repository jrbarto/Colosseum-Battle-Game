using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttack : MonoBehaviour
{
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
