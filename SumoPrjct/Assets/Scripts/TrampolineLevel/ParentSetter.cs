using UnityEngine;

public class ParentSetter : MonoBehaviour
{
    [SerializeField] private Transform playerDeformator;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HeroController>())
            playerDeformator.SetParent(transform);
        collision.transform.SetParent(transform);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.GetComponent<HeroController>())
            playerDeformator.SetParent(null);
        collision.transform.SetParent(null);
    }
}
