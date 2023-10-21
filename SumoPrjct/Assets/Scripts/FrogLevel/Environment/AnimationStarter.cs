using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStarter : MonoBehaviour
{
    [SerializeField] float startDelay;
    void Start()
    {
        Invoke(nameof(SetAnimationActive), startDelay);
    }
    private void SetAnimationActive()
    {
        GetComponent<Animation>().enabled = true;
    }
}
