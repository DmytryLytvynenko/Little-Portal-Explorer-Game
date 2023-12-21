using Sound_Player;
using UnityEngine;

public class TrampolineAnimation : MonoBehaviour
{
    [SerializeField] private SoundPlayer soundPlayer;
    private Animation m_Animation;

    private void Start()
    {
        m_Animation = GetComponent<Animation>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HeroController>()) 
        {
            soundPlayer.PlaySound(SoundName.Collision);
            m_Animation.Play();
        }
    }
}
