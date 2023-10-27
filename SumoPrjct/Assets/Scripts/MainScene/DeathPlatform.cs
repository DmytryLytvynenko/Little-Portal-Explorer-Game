using UnityEngine;

public class DeathPlatform : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(GlobalData.PlayerTag))
        {
            collision.gameObject.GetComponent<HeroController>().Die();
        }
        else
        {
            Destroy(collision.gameObject);
        }
    }
}
