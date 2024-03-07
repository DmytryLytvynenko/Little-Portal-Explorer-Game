using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTutorialTracker : MonoBehaviour
{
    [SerializeField] private FloatingJoystick controller;
    [SerializeField] private float timeToEndTutorial = 5;
    [SerializeField] private string animParameterName = "Disappear";// Параметр для переключенияя анимации в аниматоре
    private Animator tipAnimator;
    private float learnTime = 0;
    private void Start()
    {
        tipAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        if (learnTime >= timeToEndTutorial)
        {
            tipAnimator.SetBool(animParameterName, true);
        }
        else if (controller.IsWorking)
        {
            learnTime += Time.deltaTime;
        }
    }
}
