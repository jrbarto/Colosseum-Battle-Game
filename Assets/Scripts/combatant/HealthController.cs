using UnityEngine;

/*
    Controller for all aspects related to health points. This includes taking damage,
    displaying health, and dying.
*/
public class HealthController : MonoBehaviour
{
    public int maxHealthPoints;
    protected int healthPoints;
    private float damageBufferTimer = 0.5f;
    private float lastHitTime;


    void Awake() {
        if (this is EnemyHealthController) {
            gameObject.tag = "Enemy";
        } else {
            gameObject.tag = "Player";
        }
        healthPoints = maxHealthPoints;
    }

    public void TakeDamage(int damage) {
        if (Time.time - lastHitTime >= damageBufferTimer && healthPoints > 0) {
            healthPoints -= damage;
            lastHitTime = Time.time;
        }
        CombatController combatController = gameObject.GetComponent<CombatController>();
        if (this is EnemyHealthController) {
            ((EnemyHealthController)this).healthText.text = $"{healthPoints}/{maxHealthPoints}";
        }
        if (combatController != null && combatController.alive && healthPoints <= 0) {
            Die();
        }
    }

    public void HealDamage(float healPercent) {
        this.healthPoints = (int)Mathf.Clamp(
            maxHealthPoints * healPercent,
            0,
            maxHealthPoints
        );
        if (this is EnemyHealthController) {
            ((EnemyHealthController)this).healthText.text = $"{healthPoints}/{maxHealthPoints}";
        }
    }

    private void Die() {
        CombatController combatController = gameObject.GetComponent<CombatController>();
        if (combatController != null) {
            combatController.alive = false;
            combatController.dropWeapon();
        }
        Animator animator = gameObject.GetComponent<Animator>();
        animator.SetBool("dead", true);
        if (this is EnemyHealthController) {
            ((EnemyHealthController)this).Disappear();
        } else {
            MusicController musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicController>();
            musicPlayer.PlayMusic(MusicController.MusicTheme.Death);
        }
    }
}
