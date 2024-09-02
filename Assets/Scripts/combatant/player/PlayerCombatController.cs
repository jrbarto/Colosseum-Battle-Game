using UnityEngine;

public class PlayerCombatController : CombatController
{
    void Update()
    {
        if (!alive) {
            return;
        }
        if (Input.GetButtonDown("Fire1")) {
            attack();
        } else {
            animator.SetBool("attacking", false);
        }
    }
}
