using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : AnimStateMachine
{
    void Awake() {
        idleState = new EnemyIdleState(this);
        moveState = new EnemyMoveState(this);
        attackState = new EnemyAttackState(this);
        deathState = new EnemyDeathState(this);
    }
}
