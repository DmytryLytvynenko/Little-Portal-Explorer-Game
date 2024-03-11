using UnityEngine;

public class QuestTipClose : MonoBehaviour
{
    private bool anyButtonPressed = false;
    void Update()
    {
        if (Input.anyKeyDown)
        {
            anyButtonPressed = true;
        }
        if (anyButtonPressed)
            Invoke(nameof(CloseTip),1f);
    }
    private void CloseTip()
    {
        GetComponent<Animator>().SetBool("Disappear", true);
    }
}