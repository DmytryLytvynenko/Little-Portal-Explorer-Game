using UnityEngine;
using System;

public class UIAnimationTimer : MonoBehaviour
{
    [SerializeField] private SwitchTipAnimation switchTipAnimation;
    [SerializeField] private string animatorParameter;
    [Range(0, 1)]
    [SerializeField] private int boolValue;
    [SerializeField] private float delay;
    private void OnEnable()
    {
        string parameters = animatorParameter + " " + boolValue.ToString();
        StartCoroutine(switchTipAnimation.SetBool(parameters,delay));
    }
}
