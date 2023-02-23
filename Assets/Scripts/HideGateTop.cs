using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideGateTop : MonoBehaviour
{
    void OnTriggerEnter(Collider otherCollider) {
        ToggleGateTop(otherCollider.gameObject, false);
    }

    void OnTriggerExit(Collider otherCollider) {
        ToggleGateTop(otherCollider.gameObject, true);
    }

    void ToggleGateTop(GameObject collisionObject, bool active) {
        if (collisionObject.name == "Gate Sensor Activator") {
            GameObject gateTop = collisionObject.transform.parent.Find("Top Section").gameObject;
            gateTop.SetActive(active);
        }
    }
}
