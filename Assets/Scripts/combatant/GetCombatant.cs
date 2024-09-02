using UnityEngine;

/*
    This component is added to game objects that should also be considered a part of the
    combatant. Such as an arm, so that when a weapon hits that part the combatant is damaged.
    These objects are selectively chosen, and it is not sufficient to simply check if a struck
    object has the combatant as a parent. Objects such as the combatants weapon should not cause
    damage when struck.
*/
public class GetCombatant : MonoBehaviour
{
    public GameObject combatant;

    void Start() {
        if (!combatant) {
            combatant = gameObject.GetComponentInParent<HealthController>().gameObject;
        }
    }
}
