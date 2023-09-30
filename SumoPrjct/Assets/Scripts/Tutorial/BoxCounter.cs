using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCounter : MonoBehaviour
{
    [SerializeField] private int neededBoxCount;
    [SerializeField] private string objectTag;
    [SerializeField] private string animParameterName = "Disappear";// Параметр для переключенияя анимации в аниматоре
    [SerializeField] private Animator tipAnimator;
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
        tipAnimator.SetBool(animParameterName, true);
    }
}
