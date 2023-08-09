using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenu;
    public void Pause()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void Continue()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
}
