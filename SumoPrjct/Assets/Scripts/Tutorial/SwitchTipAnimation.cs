using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTipAnimation : MonoBehaviour
{
    public void SetBool(string _parameters)
    {
        Animator anim = GetComponent<Animator>();
        string[] parameters = _parameters.Split(' ');
        anim.SetBool(parameters[0], Convert.ToBoolean(Convert.ToInt32(parameters[1])));
    }
    public IEnumerator SetBool(string _parameters, float delay)
    {
        yield return new WaitForSeconds(delay);
        Animator anim = GetComponent<Animator>();
        string[] parameters = _parameters.Split(' ');
        anim.SetBool(parameters[0], Convert.ToBoolean(Convert.ToInt32(parameters[1])));
    }
}
