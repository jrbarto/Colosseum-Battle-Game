using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public bool paused = false;
    private Canvas canvas;
    private MusicController musicPlayer;


    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (canvas.enabled) {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                canvas.enabled = false;
                Time.timeScale = 1;
                paused = false;
                musicPlayer.ResumeStashedSource(true);
            } else {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                canvas.enabled = true;
                musicPlayer.StashSource();
                musicPlayer.PlayMusic(MusicController.MusicTheme.Pause, true);
                Time.timeScale = 0;
                paused = true;
            }
        }
    }
}
