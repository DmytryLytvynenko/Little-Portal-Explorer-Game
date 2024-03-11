using Sound_Player;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private SoundEffectPlayer soundPlayer;
    public void PauseUnpause()
    {
        if (PauseMenu == null)
            return;

        if(PauseMenu.activeSelf)
        {
            PauseMenu.SetActive(false);
            CursorHider.HideCursor();
            Time.timeScale = 1;
        }
        else
        {
            PauseMenu.SetActive(true);
            CursorHider.ShowCursor();
            Time.timeScale = 0;
        }
        soundPlayer.PlaySound(SoundName.Click);
    }
    public void Continue()
    {
        PauseMenu.SetActive(false);
        CursorHider.HideCursor();
        soundPlayer.PlaySound(SoundName.Click);
        Time.timeScale = 1;
    }

}
