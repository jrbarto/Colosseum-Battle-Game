using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{
    protected AnimStateMachine stateMachine;
    protected Animator animator;
    protected bool twoHandedWeapon;

    public BaseState(AnimStateMachine stateMachine) {
        this.stateMachine = stateMachine;
        this.animator = stateMachine.animator;
        this.twoHandedWeapon = animator.GetBool("twoHandedWeapon");
    }

    public virtual void Enter() {}
    public virtual void Update() {}
    public virtual string[] GetAnimatorStates() { return null; }

    protected void moveToAnimatorState(string animatorState, float transitionTime) {
        bool runningCurrStateAnim = areAnimStatesActive(this.GetAnimatorStates(), true);
        if (isAnimatorStateActive(animatorState)) {
            this.animator.Play(animatorState, 0, 0f);
            this.animator.CrossFade(animatorState, transitionTime);
        } else if (!isAnimatorStateActive(animatorState, true)) {
            this.animator.CrossFade(animatorState, transitionTime);
        }
    }

    // return true if any of the specified animations are playing
    protected bool areAnimStatesActive(string[] animStates, bool isPlaying) {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);  // Cache state info
        AnimatorTransitionInfo transitionInfo = animator.GetAnimatorTransitionInfo(0);  // Cache transition info
        bool isNewState = this != this.stateMachine.currentState;

        //Debug.Log("IS NEW STATE ? " + isNewState);
        // Loop through the animations and check if the current or transition state matches
        foreach (string animation in animStates) {
            if (animator.IsInTransition(0)) {
                // Debug.Log("IN TRANSITION!");
                return true;
            } else {
                if (stateInfo.IsName(animation)) {
                    if (isPlaying) {
                        return stateInfo.normalizedTime < 1.0f;
                    } else {
                        return true;
                    }
                }
            }
        }

        // If no match was found
        // Debug.Log("NOT PLAYING ANY OF THE ANIMATIONS.");
        return false;
    }

}
