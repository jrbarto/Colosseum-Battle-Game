using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimStateMachine : MonoBehaviour
{
    public BaseState currentState;
    public Animator animator;
    public BaseState idleState;
    public BaseState moveState;
    public BaseState attackState;
    public BaseState deathState;

    void Start(){
        animator = GetComponent<Animator>();
        currentState = idleState;
        currentState.Enter();
    }

    void Update() {
        if (this.currentState != null) {
            this.currentState.Update();
        }
    }

    public void SetState(BaseState state) {
        bool alive = GetComponent<CombatController>().alive;
        if (!alive) {
            state = deathState;
        }
        //Debug.Log("COMPARING CURRENT STATE " + currentState + " WITH NEW STATE " + state);
        if (currentState != state) {
            currentState = state;
            //Debug.Log("ENTERING NEW STATE " + currentState);
            currentState.Enter();
        }
    }
}
