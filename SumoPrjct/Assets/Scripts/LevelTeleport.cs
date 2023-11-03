using UnityEngine;

public class LevelTeleport : MonoBehaviour
{
    [SerializeField] private string levelName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        LoadLevel();
    }

    private void LoadLevel()
    {
        Scenes.LoadSN(levelName);
    }
}
