using UnityEngine;

public class GetCombatant : MonoBehaviour
{
    public GameObject combatant;

    void Start() {
        combatant = gameObject.GetComponentInParent<HealthController>().gameObject;
    }
}
