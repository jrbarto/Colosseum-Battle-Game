using UnityEngine;
using UnityEngine.AI;

public class EnemyLevelController : MonoBehaviour
{
    public GameObject weapon;
    public AnimationClip attackClip;
    public Transform centerBone;
    public int level;
    private Transform weaponTip;
    private Animator animator;
    private EnemyStats stats;

    // TODO: add a leveling function that uses the enemy's current level and spreads it 
    // around through the following attributes (level 0 is base stats):
    // turningSpeed, acceleration, maxSpeed, damage, weaponLength, maxHealthPoints, stamina (when i add it)

    void Start()
    {
        stats = new EnemyStats();
        this.setLevel(level); //remove when i move this to enemyspawner
        weaponTip = Utils.FindChildByName(weapon.transform, "weapon-tip").transform;
        animator = GetComponent<Animator>();
        float attackRange = calcAttackRange();
        weapon.GetComponent<WeaponAttack>().attackRange = attackRange;
        
    }

    public void setLevel(int level) {
        this.level = level;
        stats.randomizeStats(level);
        this.configureWeaponLength(stats.weaponLength);
        this.configureWeaponStats(stats.damage, stats.attackSpeed);
        this.configureMovement(stats.turningSpeed, stats.acceleration, stats.maxSpeed);
        this.configureHealth(stats.maxHealthPoints);
    }

    private void configureWeaponLength(int weaponLength) {
        Transform weaponOrigin = weapon.transform.Find("weapon-origin");
        float newYScale = weaponOrigin.localScale.y + (weaponLength * 0.2f);
        weaponOrigin.localScale = new Vector3(
            weaponOrigin.localScale.x,
            newYScale,
            weaponOrigin.localScale.z
        );
    }

    private void configureWeaponStats(int damage, int speed) {
        WeaponAttack weaponAttack = weapon.GetComponent<WeaponAttack>();
        weaponAttack.damage += damage;
        weaponAttack.attackSpeed += speed;
    }

    private void configureMovement(int turningSpeed, int acceleration, int maxSpeed) {
        EnemyCombatController combatController = GetComponent<EnemyCombatController>();
        combatController.turningSpeed += turningSpeed * 0.5f;
        combatController.acceleration += acceleration * 0.5f;
        GetComponent<NavMeshAgent>().speed += maxSpeed * 0.5f;
    }

    private void configureHealth(int maxHealthPoints) {
        EnemyHealthController healthController = GetComponent<EnemyHealthController>();
        healthController.maxHealthPoints += maxHealthPoints;
        healthController.HealDamage(1.0f);
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
