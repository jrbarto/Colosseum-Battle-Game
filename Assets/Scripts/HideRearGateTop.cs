using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideRearGateTop : MonoBehaviour
{
    void OnTriggerEnter(Collider otherCollider) {
        ToggleGateTop(otherCollider.gameObject, false);
    }

    void OnTriggerExit(Collider otherCollider) {
        ToggleGateTop(otherCollider.gameObject, true);
    }

    void ToggleGateTop(GameObject collisionObject, bool active) {
        if (collisionObject.name == "Gate Sensor Activator") {
            GameObject rearGateTop = collisionObject.transform.parent.Find("Top Section").gameObject;
            rearGateTop.SetActive(active);
        }
    }
}
