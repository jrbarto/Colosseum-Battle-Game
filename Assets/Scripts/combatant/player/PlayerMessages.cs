using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerMessages : MonoBehaviour
{
    private TextMeshProUGUI messages;

    void Awake () {
        messages = GetComponent<TextMeshProUGUI>();
        messages.SetText("");
    }

    public void ShowMessage (string message) {
        messages.SetText(message);
        StartCoroutine(ClearMessageAfterSeconds(5));
    }

    private IEnumerator ClearMessageAfterSeconds (int seconds) {
        yield return new WaitForSeconds(seconds);
        messages.SetText("");
    }
}
