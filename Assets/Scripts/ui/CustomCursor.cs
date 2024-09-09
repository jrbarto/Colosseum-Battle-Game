using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Vector2 hotspot;
    public CursorMode cursorMode;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotspot, cursorMode);
    }
}
