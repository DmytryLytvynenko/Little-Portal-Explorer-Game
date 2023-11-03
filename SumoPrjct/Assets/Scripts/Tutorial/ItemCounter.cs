using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCounter : MonoBehaviour
{
    [SerializeField] private int neededBoxCount;
    [SerializeField] private string objectTag;
    [SerializeField] private string animParameterName = "Disappear";// Параметр для переключенияя анимации в аниматоре
    [SerializeField] private Animator tipAnimator;
    [SerializeField] private Rigidbody[] gates;
    private int counter = 0;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(objectTag))
            return;

        counter++;
        print(counter);
        if (counter >= neededBoxCount)
        {
            Invoke(nameof(EndBoxThrowTutorial), 1.5f);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(objectTag))
            return;

        counter--;
        print(counter);
    }
    private void EndBoxThrowTutorial()
    {
        foreach (var gate in gates)
            gate.isKinematic = false;

        tipAnimator.SetBool(animParameterName, true);
    }
}
