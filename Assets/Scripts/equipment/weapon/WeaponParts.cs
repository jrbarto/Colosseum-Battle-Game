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

    public Dictionary<ResourceType, int> GetWeaponResources() {
        float decimalPercent = weaponScorePercent / 100f;
        Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int> {
            { ResourceType.Bronze, (int)(maxPayouts[ResourceType.Bronze] * decimalPercent) }
        };
        if (weaponScorePercent > 10) {
            resources.Add(ResourceType.Iron, (int)(maxPayouts[ResourceType.Iron] * decimalPercent));
        }
        if (weaponScorePercent > 20) {
            resources.Add(ResourceType.Gold, (int)(maxPayouts[ResourceType.Gold] * decimalPercent));
        }
        if (weaponScorePercent > 30) {
            resources.Add(ResourceType.Platinum, (int)(maxPayouts[ResourceType.Platinum] * decimalPercent));
        }

        return resources;
    }

    public void CalcWeaponScorePercentage(EnemyStats enemyStats) {
        float totalScore = 0;
        foreach (StatType statName in EnemyStats.maxValues.Keys) {
            int value = enemyStats.stats[statName] == 0  ? 1 : enemyStats.stats[statName];
            totalScore += ((float)value / EnemyStats.maxValues[statName]) * 100;
        }

        this.weaponScorePercent = totalScore / EnemyStats.maxValues.Count;
        Debug.Log("Weapon score percent is " + this.weaponScorePercent);
    }
}
