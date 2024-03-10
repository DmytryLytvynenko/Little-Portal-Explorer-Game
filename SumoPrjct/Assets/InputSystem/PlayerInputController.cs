using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private HeroController heroController;
    private PlayerInput inputs;

    private void Awake()
    {
        inputs = new PlayerInput();

        inputs.Player.Attack.performed += context => heroController.StartAttackAnimation();
        inputs.Player.Throw.performed += context => heroController.StartThrowAnimation();
        inputs.Player.Jump.performed += context => heroController.StartJumpAnimation();
        inputs.Player.Explode.performed += context => heroController.Explode();
    }
    private void OnEnable()
    {
        inputs.Enable();
    }
    private void OnDisable()
    {
        inputs.Disable();
    }

    private void Update()
    {
        Vector2 direction = inputs.Player.Move.ReadValue<Vector2>();
        print(direction);
        Vector3 moveDirection = new Vector3(direction.x,0f,direction.y);
        heroController.Move(moveDirection);
    }
}
