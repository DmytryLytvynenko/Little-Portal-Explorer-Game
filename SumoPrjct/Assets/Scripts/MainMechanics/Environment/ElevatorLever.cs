using UnityEngine;
using UnityEngine.UI;

public class ElevatorLever : MonoBehaviour
{
    enum AnimatorParameter {Switch}
    [SerializeField] private Button button;
    [SerializeField] private Elevator elevator;

    private Animator animator;
    private void OnEnable()
    {
        elevator.ElevatorMoveEnded += OnElevatorMoveEnded;        
    }
    private void OnDisable()
    {
        elevator.ElevatorMoveEnded -= OnElevatorMoveEnded;   
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void StartSwitchAnimation()
    {
        animator.SetTrigger(AnimatorParameter.Switch.ToString());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            button.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            button.gameObject.SetActive(false);
        }
    }
    private void OnElevatorMoveEnded()
    {
        button.interactable = true;
    }
}
