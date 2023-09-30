using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenes : MonoBehaviour
{
    public void LoadSN(int sceneNumber)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
    }
    public void LoadSN(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
