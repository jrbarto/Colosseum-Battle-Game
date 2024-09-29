using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : BaseState
{
    private string deathState = "Base Layer.Death";

    public EnemyDeathState(AnimStateMachine stateMachine): base(stateMachine) {}

    public override void Enter() {
        this.animator.CrossFade(deathState, 0.2f);
    }


    public override void Update() {
        // nothing to do, you're dead
    }
}
