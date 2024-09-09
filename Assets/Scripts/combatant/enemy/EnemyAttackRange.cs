using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    public WeaponAttack weaponAttack;
    public GameObject weaponTip;
    public AnimationClip attackClip;
    public GameObject centerBone;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        float attackRange = calcAttackRange();
        weaponAttack.attackRange = attackRange;
    }

    private float calcAttackRange() {
        float maxDistance = 0;
        float animationLength = attackClip.length;
        int sampleCount = 100;  // number of samples to take during the animation

        for (int sampleIndex = 0; sampleIndex <= sampleCount; sampleIndex++) {
            float time = (sampleIndex / (float)sampleCount) * animationLength;
            attackClip.SampleAnimation(gameObject, time);  // sample the animation at the current time

            // calculate distance from center of enemy (center bone) to the tip of the weapon mid animation swing
            Vector3 originPoint = centerBone.transform.position;
            float distance = Vector3.Distance(originPoint, weaponTip.transform.position);
            if (distance > maxDistance) {
                maxDistance = distance;
            }
        }

        return maxDistance;
    }
}
