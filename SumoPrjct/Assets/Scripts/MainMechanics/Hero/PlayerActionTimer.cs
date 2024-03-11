using UnityEngine;

public class PlayerActionTimer : MonoBehaviour
{
    [SerializeField] private float coolDowm = 1f;

    public bool ActionCanBePerformed { get; private set; } = true;

    public void OnActionPerformed()
    {
        ActionCanBePerformed = false;
        Invoke(nameof(AllowPerformActions), coolDowm);
    }
    private void AllowPerformActions()
    {
        ActionCanBePerformed = true;
    }
}
