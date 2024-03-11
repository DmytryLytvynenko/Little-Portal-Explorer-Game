using UnityEngine;

public class FPSSetter : MonoBehaviour
{
    [SerializeField] private int TargetFPSCount = 60;

    private void OnValidate()
    {
        Application.targetFrameRate = TargetFPSCount;
    }
}