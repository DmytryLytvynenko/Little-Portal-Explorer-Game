using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    [SerializeField] private Elevator elevator;
    [SerializeField] private MeshRenderer renderer;
    [SerializeField] private Material free;
    [SerializeField] private Material busy;

    private Animator animator;
    private void OnEnable()
    {
        elevator.ElevatorMoveEnded += OnElevatorMoveEnded;
    }
    private void OnDisable()
    {
        elevator.ElevatorMoveEnded -= OnElevatorMoveEnded;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            if (elevator.AtStartPosition)
                return;

            elevator.ActivateElevator();
            renderer.material = busy;
        }
    }
    private void OnElevatorMoveEnded()
    {
        renderer.material = free;
    }
}
