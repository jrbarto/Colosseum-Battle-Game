using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using Game.Resources;
using Game.Enemy;

public class WeaponParts : MonoBehaviour
{
    private Dictionary<ResourceType, float> maxPayouts = new Dictionary<ResourceType, float> {
        { ResourceType.Bronze, 50f },
        { ResourceType.Iron, 40f },
        { ResourceType.Gold, 30f },
        { ResourceType.Platinum, 20f }
    };
    public float weaponScorePercent;
    // TODO Add resourceType variable and functions to calculate how many resources
    // a weapon has based on WeaponAttack. And function to add to PlayerEquipment.

    public Dictionary<ResourceType, int> GetWeaponResources() {
        float decimalPercent = weaponScorePercent / 100f;
        Debug.Log("Weapon score percent is " + weaponScorePercent);
        return new Dictionary<ResourceType, int> {
            { ResourceType.Bronze, (int)(maxPayouts[ResourceType.Bronze] * decimalPercent) },
            { ResourceType.Iron, (int)(maxPayouts[ResourceType.Iron] * decimalPercent) },
            { ResourceType.Gold, (int)(maxPayouts[ResourceType.Gold] * decimalPercent) },
            { ResourceType.Platinum, (int)(maxPayouts[ResourceType.Platinum] * decimalPercent) }
        };
    }

    public void CalcWeaponScorePercentage(EnemyStats enemyStats) {
        float totalScore = 0;
        foreach (KeyValuePair<StatType, int> entry in EnemyStats.maxValues) {
            Debug.Log(entry.Key + " : " + entry.Value);
        }
        foreach (StatType statName in EnemyStats.maxValues.Keys) {
            int value = enemyStats.stats[statName] == 0  ? 1 : enemyStats.stats[statName];
            totalScore += ((float)value / EnemyStats.maxValues[statName]) * 100;
        }

        this.weaponScorePercent = totalScore / EnemyStats.maxValues.Count;
    }
}
