using UnityEditor;
using UnityEngine;

public class LevelTeleport : MonoBehaviour
{
    [SerializeField] private SceneAsset level;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        LoadLevel();
    }

    private void LoadLevel()
    {
        Scenes.LoadSN(level);
    }
}
