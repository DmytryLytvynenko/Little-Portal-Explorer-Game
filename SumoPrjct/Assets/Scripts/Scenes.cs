using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Scenes : MonoBehaviour
{
    public static void LoadSN(int sceneNumber)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneNumber, LoadSceneMode.Single);
    }
    public static void LoadSN(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
    public static void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
