using UnityEngine;

public class JumpTutorialEnded : MonoBehaviour
{
    [SerializeField] private GameObject throwTip;
    private bool isTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            isTriggered = true;
            throwTip.SetActive(true);
        }
    }
}