using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : BaseState
{
    private string onehandWalkState = "Onehand Movement.Walking";
    private string onehandRunState = "Onehand Movement.Running";
    private string twohandWalkState = "Twohand Movement.Walking";
    private string twohandRunState = "Twohand Movement.Running";

    public EnemyMoveState(AnimStateMachine stateMachine): base(stateMachine) {}

    public override void Enter() {
        string walkState = this.twoHandedWeapon ? twohandWalkState : onehandWalkState;
        this.animator.CrossFade(walkState, 0.2f);
    }

    public override void Update() {
        string[] attacks = new string[] { 
            onehandWalkState, 
            onehandRunState, 
            twohandWalkState, 
            twohandRunState 
        };
        if (!this.areAnimationsPlaying(attacks)) {
            this.animator.Play(onehandWalkState, 0, 0f);
            this.animator.CrossFade(onehandWalkState, 0.85f);
        }
    }
}
