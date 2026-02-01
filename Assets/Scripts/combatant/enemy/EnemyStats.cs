using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using Game.Enemy;

using System;

public class EnemyStats : MonoBehaviour
{
    public Dictionary<StatType, int> stats = new Dictionary<StatType, int>();
    public static Dictionary<StatType, int> maxValues = new Dictionary<StatType, int> {
        { StatType.TurningSpeed, 20 },
        { StatType.Acceleration, 50 },
        { StatType.MaxSpeed, 20 },
        { StatType.Damage, 20 },
        { StatType.AttackSpeed, 10 },
        { StatType.WeaponLength, 10 },
        { StatType.MaxHealthPoints, 20 },
        { StatType.Stamina, 20 }
    };


    public EnemyStats() {
        foreach (StatType statName in Enum.GetValues(typeof(StatType))) {
            this.stats[statName] = 0;
        }
    }

    public void randomizeStats(int statPoints) {
        List<StatType> underMaxStats = new List<StatType>();

        foreach (StatType statName in EnemyStats.maxValues.Keys) {
            int maxValue = EnemyStats.maxValues[statName];
            if (stats[statName] < maxValue) {
                underMaxStats.Add(statName);
            }
        }
        for (int i = 0; i < statPoints && underMaxStats.Count > 0; i++) {
            int random = UnityEngine.Random.Range(0, underMaxStats.Count - 1);
            StatType statName = underMaxStats[random];
            int currentValue = this.stats[statName];
            this.stats[statName] += 1;
            if (currentValue + 1 >= EnemyStats.maxValues[statName]) {
                underMaxStats.RemoveAll(underMaxStat => underMaxStat == statName);
            }
        }

        Debug.Log("Randomized stats: turningSpeed: " + stats[StatType.TurningSpeed] + 
            " acceleration: " + stats[StatType.Acceleration] +
            " maxSpeed: " + stats[StatType.MaxSpeed] +
            " damage: " + stats[StatType.Damage] + 
            " attackSpeed: " + stats[StatType.AttackSpeed] +
            " weaponLength: " + stats[StatType.WeaponLength] +
            " maxHealthPoints: " + stats[StatType.MaxHealthPoints] +
            " stamina: " + stats[StatType.Stamina]
        );
    }
}
