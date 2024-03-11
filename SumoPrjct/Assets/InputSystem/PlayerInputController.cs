using UnityEngine;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private HeroController heroController;
    [SerializeField] private CameraRotate cameraRotate;
    [SerializeField] private PauseGame pauseGame;

    private PlayerInput inputs;

    private void Awake()
    {
        inputs = new PlayerInput();

        inputs.Player.Attack.performed += context => heroController.StartAttackAnimation();
        inputs.Player.Throw.performed += context => heroController.StartThrowAnimation();
        inputs.Player.Jump.performed += context => heroController.StartJumpAnimation();
        inputs.Player.Interact.performed += context => heroController.StartInteractAnimation();
        inputs.Player.Explode.performed += context => heroController.Explode();
        inputs.Player.Pause.performed += context => pauseGame.PauseUnpause();
    }
    private void OnEnable()
    {
        inputs.Enable();
    }
    private void OnDisable()
    {
        inputs.Disable();
    }
    private void FixedUpdate()
    {
        ReadMoveDirection();
    }
    private void LateUpdate()
    {
        ReadRotationInput();
    }
    private void ReadMoveDirection()
    {
        Vector2 directionInput = inputs.Player.Move.ReadValue<Vector2>();
        Vector3 moveDirection = new Vector3(directionInput.x, 0f, directionInput.y);
        heroController.moveVector = moveDirection;
    }
    private void ReadRotationInput()
    {
        Vector2 rotationInput = inputs.Player.RotateCamera.ReadValue<Vector2>();
        cameraRotate.xRotation = rotationInput.y;
        cameraRotate.yRotation = rotationInput.x;
    }
}