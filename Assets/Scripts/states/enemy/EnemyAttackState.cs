using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : BaseState
{
    private string oneHandDownAttackState = "Base Layer.Onehand Attacking.Down Swing";
    private string oneHandBackAttackState = "Base Layer.Onehand Attacking.Back Swing";
    private string twoHandDownAttackState = "Base Layer.Twohand Attacking.Down Swing";
    private string twoHandBackAttackState = "Base Layer.Twohand Attacking.Back Swing";

    public EnemyAttackState(AnimStateMachine stateMachine): base(stateMachine) {}

    public override void Enter() {
        // Debug.Log("ENTERING ATTACK STATE!");
        this.selectAttack();
    }

    public override void Update() {
        this.selectAttack();
    }

    public override string[] GetAnimatorStates() {
        return new string[] { 
            oneHandDownAttackState, 
            oneHandBackAttackState, 
            twoHandDownAttackState, 
            twoHandBackAttackState 
        };
    }

    private void selectAttack() {
        List<string> attacks;
        if (this.twoHandedWeapon) {
            attacks = new List<string> { twoHandDownAttackState, twoHandBackAttackState };
        } else {
            attacks = new List<string> { oneHandDownAttackState, oneHandBackAttackState };
        }
        
        int random = Random.Range(0, attacks.Count);
        this.moveToAnimatorState(attacks[random], 0.2f);
    }
}
