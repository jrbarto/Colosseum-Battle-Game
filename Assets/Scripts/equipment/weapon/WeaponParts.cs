using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using Game.Resources;
using Game.Enemy;

public class WeaponParts : MonoBehaviour
{
    public int weaponScorePercent;
    // TODO Add resourceType variable and functions to calculate how many resources
    // a weapon has based on WeaponAttack. And function to add to PlayerEquipment.

    void Start() {
        this.weaponScorePercent = this.CalcWeaponScorePercentage();
    }

    int CalcWeaponScorePercentage() {
        EnemyStats enemyStats = transform.GetComponentInParent<EnemyLevelController>().enemyStats;
        int totalScore = 0;
        foreach (StatType statName in EnemyStats.maxValues.Keys) {
            int value = enemyStats.stats[statName];
            totalScore += (value / EnemyStats.maxValues[statName]) * 100;
        }

        return totalScore / EnemyStats.maxValues.Count;
    }
}
