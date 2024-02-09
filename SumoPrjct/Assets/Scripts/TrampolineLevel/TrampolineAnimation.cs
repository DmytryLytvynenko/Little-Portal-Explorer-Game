using Sound_Player;
using UnityEngine;

public class TrampolineAnimation : MonoBehaviour
{
    [SerializeField] private SoundEffectPlayer soundEffectPlayer;
    private Animation m_Animation;

    private void Start()
    {
        m_Animation = GetComponent<Animation>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<HeroController>()) 
        {
            soundEffectPlayer.PlaySound(SoundName.Collision);
            m_Animation.Play();
        }
    }
}
