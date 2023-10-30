using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] private HealthControll PlayerHealth;
    public void RespawnPlayer()
    {
        Utilities.Utilities.SetPlayerPosition();
        PlayerHealth.RestoreHealth();
        gameObject.SetActive(false);
    }
}
