using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : BaseState
{
    private string idleState = "Idle";

    public EnemyIdleState(AnimStateMachine stateMachine): base(stateMachine) {}

    public override void Enter() {
        this.animator.CrossFade(idleState, 0.2f);
    }

    public override void Update() {
        // TODO
    }
}
