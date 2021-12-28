using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    public Texture2D cursorTexture;
    void Start()
    {
        Vector2 newpost = Vector2.zero;
        if(cursorTexture != null)
        {
            newpost = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        }
        Cursor.SetCursor(cursorTexture, newpost, CursorMode.ForceSoftware);
    }
}
