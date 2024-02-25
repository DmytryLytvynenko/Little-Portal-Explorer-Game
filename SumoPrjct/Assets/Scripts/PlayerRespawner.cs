using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    [SerializeField] private HealthControll PlayerHealth;
    [SerializeField] private Animator playerAnimator;
    public void RespawnPlayer()
    {
        Utilities.Utilities.SetPlayerPosition();
        PlayerHealth.RestoreHealth();
        playerAnimator.Rebind();
        playerAnimator.Update(0f);
    }
}
