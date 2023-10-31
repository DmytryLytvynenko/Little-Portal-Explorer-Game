using UnityEngine;

public class TrampolineAnimation : MonoBehaviour
{
    private Animation m_Animation;

    private void Start()
    {
        m_Animation = GetComponent<Animation>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HeroController>()) 
        {
            m_Animation.Play();
        }
    }
}
