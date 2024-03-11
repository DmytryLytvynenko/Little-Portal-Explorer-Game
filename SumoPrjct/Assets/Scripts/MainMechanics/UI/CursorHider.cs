using UnityEngine;

public class CursorHider : MonoBehaviour
{
    [SerializeField] private bool hideCursorOnStart = true;
    private void Start()
    {
        if (hideCursorOnStart)
        {
            HideCursor();
        }
        else
        {
            ShowCursor();
        }
    }
    public static void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public static void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }
}
