using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    GameObject enemy;
    public int respawnSeconds = 30;
    private int level;
    private int? timer;

    void Awake() {
        gameObject.tag = "EnemySpawner";
    }

    // Start is called before the first frame update
    void Start()
    {
        level = 0;
    }

    void OnGUI() {
        if (timer != null) {
            int cursorSize = 1000;
            int cursorX = Camera.main.pixelWidth / 2;
            int cursorY = Camera.main.pixelHeight / 20;
            GUIStyle boxStyle = new GUIStyle();
            boxStyle.fontSize = Camera.main.pixelHeight / 20;
            boxStyle.fontStyle = FontStyle.Bold;
            float timeRatio = ((float)timer)/respawnSeconds;
            if (timeRatio >= 0.7f) {
                boxStyle.normal.textColor = Color.green;
            } else if (timeRatio >= 0.3f) {
                boxStyle.normal.textColor = Color.yellow;
            } else {
                boxStyle.normal.textColor = Color.red;
            }
            GUI.Label(new Rect(cursorX, cursorY, cursorSize, cursorSize), timer.ToString(), boxStyle);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy == null) {
            enemy = Instantiate(enemyPrefabs[level % enemyPrefabs.Length]);
            enemy.transform.position = new Vector3(transform.position.x, enemy.transform.position.y, transform.position.z);
            enemy.transform.rotation = transform.rotation;
            level += 1;
        }
    }

    public IEnumerator DespawnEnemy(GameObject deadEnemy) {
        timer = respawnSeconds;
        while (timer >=0) {
            yield return new WaitForSeconds(1);
            timer -= 1;
        }
        timer = null;
        GameObject.Destroy(deadEnemy);
    }
}
