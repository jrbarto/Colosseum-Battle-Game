using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLevelController : MonoBehaviour
{
    public GameObject weapon;
    public AnimationClip attackClip;
    public Transform centerBone;
    private Transform weaponTip;
    private Animator animator;

    void Start()
    {
        weaponTip = Utils.FindChildByName(weapon.transform, "weapon-tip").transform;
        animator = GetComponent<Animator>();
        float attackRange = calcAttackRange();
        weapon.GetComponent<WeaponAttack>().attackRange = attackRange;
    }

    private float calcAttackRange() {
        float maxDistance = 0;
        float animationLength = attackClip.length;
        int sampleCount = 100;  // number of samples to take during the animation

        for (int sampleIndex = 0; sampleIndex <= sampleCount; sampleIndex++) {
            float time = (sampleIndex / (float)sampleCount) * animationLength;
            attackClip.SampleAnimation(gameObject, time);  // sample the animation at the current time

            // calculate distance from center of enemy (center bone) to the tip of the weapon mid animation swing
            Vector3 originPoint = centerBone.position;
            float distance = Vector3.Distance(originPoint, weaponTip.position);
            if (distance > maxDistance) {
                maxDistance = distance;
            }
        }

        return maxDistance;
    }
}
