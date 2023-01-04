using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    GameObject enemy;
    private int level;

    // Start is called before the first frame update
    void Start()
    {
        level = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null) {
            enemy = Instantiate(enemyPrefabs[level % enemyPrefabs.Length]);
            level += 1;
        }
    }
}
