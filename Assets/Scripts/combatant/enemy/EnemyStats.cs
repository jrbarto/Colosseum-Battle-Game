using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

using System.ComponentModel;

public class EnemyStats : MonoBehaviour
{
    public int turningSpeed { get; set; }
    public int acceleration { get; set; }
    public int maxSpeed { get; set; }
    public int damage { get; set; }
    public int attackSpeed { get; set; }
    public int weaponLength { get; set; }
    public int maxHealthPoints { get; set; }
    public int stamina { get; set; }
    private Dictionary<string, int> maxValues = new Dictionary<string, int> {
        {"turningSpeed", 20},
        {"acceleration", 20},
        {"maxSpeed", 20},
        {"damage", 20},
        {"attackSpeed", 20},
        {"weaponLength", 10},
        {"maxHealthPoints", 20},
        {"stamina", 20}
    };


    public EnemyStats() {
        this.turningSpeed = 0;
        this.acceleration = 0;
        this.maxSpeed = 0;
        this.damage = 0;
        this.attackSpeed = 0;
        this.weaponLength = 0;
        this.maxHealthPoints = 0;
        this.stamina = 0;
    }

    public void randomizeStats(int level) {
        List<PropertyInfo> underMaxProps = new List<PropertyInfo>();

        foreach (string propName in this.maxValues.Keys) {
            int maxValue = this.maxValues[propName];
            PropertyInfo prop = typeof(EnemyStats).GetProperty(propName);
            if ((int)prop.GetValue(this) < maxValue) {
                underMaxProps.Add(prop);
            }
        }
        for (int i = 0; i < level && underMaxProps.Count > 0; i++) {
            int random = Random.Range(0, underMaxProps.Count);
            PropertyInfo prop = underMaxProps[random];
            int currentValue = (int)prop.GetValue(this);
            prop.SetValue(this, currentValue + 1);
            if (currentValue + 1 >= maxValues[prop.Name]) {
                underMaxProps.RemoveAll(underMaxProp => underMaxProp.Name == prop.Name);
            }
        }

        Debug.Log("Randomized stats: turningSpeed: " + turningSpeed + 
            " acceleration: " + acceleration +
            " maxSpeed: " + maxSpeed +
            " damage: " + damage + 
            " attackSpeed: " + attackSpeed +
            " weaponLength: " + weaponLength +
            " maxHealthPoints: " + maxHealthPoints +
            " stamina: " + stamina
        );
    }
}
