using UnityEngine;

public class ToggleCursor : MonoBehaviour
{
    private bool cursorVisible = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            cursorVisible = !cursorVisible;
            Cursor.visible = cursorVisible;
            Cursor.lockState = cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}
