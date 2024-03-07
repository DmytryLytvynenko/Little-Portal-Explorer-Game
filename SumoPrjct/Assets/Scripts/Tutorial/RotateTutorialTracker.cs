using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTutorialTracker : MonoBehaviour
{
    [SerializeField] private CameraRotate controller;
    [SerializeField] private float timeToEndTutorial = 3;
    [SerializeField] private string animParameterName = "Disappear";// Параметр для переключенияя анимации в аниматоре
    [SerializeField] private GameObject moveTip;
    [SerializeField] private HeroController heroController;
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
            moveTip.SetActive(true);
            heroController.enabled = true;
        }
        else if (controller.IsWorking)
        {
            learnTime += Time.deltaTime;
        }
    }
}
