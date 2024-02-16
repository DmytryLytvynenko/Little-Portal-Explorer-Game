using UnityEngine;

public class PortalController : MonoBehaviour
{
    [SerializeField] Animation portalAnimation;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            portalAnimation.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            portalAnimation.enabled = false;
        }
    }
}
