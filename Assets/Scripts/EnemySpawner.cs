using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    GameObject enemy;
    GameObject rearGate;
    MusicController musicPlayer;
    public int respawnSeconds = 30;
    private int level;
    private int? timer;

    void Awake() {
        gameObject.tag = "EnemySpawner";
    }

    void Start()
    {
        level = 0;
        rearGate = GameObject.FindGameObjectWithTag("RearGate");
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicController>();
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

    void Update()
    {
        if (enemy == null) {
            rearGate.GetComponent<GateControl>().SetGateDirection(1);
            enemy = Instantiate(enemyPrefabs[level % enemyPrefabs.Length]);
            StartCoroutine(WaitToActivateEnemy(enemy.GetComponentInChildren<EnemyCombatController>()));
            enemy.transform.position = new Vector3(transform.position.x, enemy.transform.position.y, transform.position.z);
            enemy.transform.rotation = transform.rotation;
            level += 1;
        }
    }

    public IEnumerator WaitToActivateEnemy(EnemyCombatController controller) {
        musicPlayer.PlayMusic(true);
        controller.enabled = false;
        yield return new WaitForSeconds(5);
        controller.enabled = true;
    }

    public IEnumerator DespawnEnemy(GameObject deadEnemy) {
        musicPlayer.PlayMusic(false);
        timer = respawnSeconds;
        while (timer >=0) {
            yield return new WaitForSeconds(1);
            timer -= 1;
        }
        timer = null;
        GameObject.Destroy(deadEnemy);
    }
}
