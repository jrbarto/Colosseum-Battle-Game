using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int healthPoints = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage) {
        if (healthPoints > 0) {
            healthPoints -= damage;
        }
        Debug.Log("ENEMY HEALTH AT " + healthPoints);
    }
}
