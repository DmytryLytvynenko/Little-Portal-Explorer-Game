using Sirenix.OdinInspector;
using UnityEngine;

public class MenuPlayButton : MonoBehaviour
{
    public void Play()
    {
        if (PlayerPrefs.HasKey("CheckPointPosition"))
            SceneTransition.SwitchToScene("MainLevel");
        else
            SceneTransition.SwitchToScene("Tutorial");
    }
    [Button("DeletePlayerCheckPoint")]
    private void DeletePlayerCheckPoint()
    {
        PlayerPrefs.DeleteKey("CheckPointPosition");
        PlayerPrefs.DeleteKey("CheckPointName");
    }
}