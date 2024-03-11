using Sound_Player;
using UnityEngine;
using UnityEngine.UI;

public class ElevatorLever : MonoBehaviour
{
    enum AnimatorParameter {Switch}
    [SerializeField] private GameObject message;
    [SerializeField] private Elevator elevator;

    private Animator animator;
    private HeroController heroController;
    private SoundEffectPlayer soundPlayer;
    private bool interactable = true;
    private void Awake()
    {
        heroController = FindAnyObjectByType<HeroController>();
        animator = GetComponent<Animator>();
        soundPlayer = GetComponent<SoundEffectPlayer>();
    }
    private void OnEnable()
    {
        elevator.ElevatorMoveEnded += OnElevatorMoveEnded;
        HeroController.PlayerInteracted += StartSwitchAnimation;
    }
    private void OnDisable()
    {
        HeroController.PlayerInteracted -= StartSwitchAnimation;
        elevator.ElevatorMoveEnded -= OnElevatorMoveEnded;   
    }
    public void StartSwitchAnimation()
    {
        if (!interactable)
            return;

        if ((heroController.transform.position - transform.position).magnitude < 1)
        {
            animator.SetTrigger(AnimatorParameter.Switch.ToString());
            elevator.ActivateElevator();
            soundPlayer.PlaySound(SoundName.Switch);
            interactable = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            message.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            message.SetActive(false);
        }
    }
    private void OnElevatorMoveEnded()
    {
        interactable = true;
    }
}
