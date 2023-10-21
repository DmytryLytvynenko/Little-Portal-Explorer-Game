using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlluminationBlinking : MonoBehaviour
{
    [SerializeField] private AnimationCurve illuminationIntencivity;
    [SerializeField] private float duration = 0;
    [SerializeField] private Light[] lightSources;
    private float progress = 0;
    private float expiredTime = 0;

    void Update()
    {
        ChangeIlluminationIntencivity();
    }
    private void ChangeIlluminationIntencivity()
    {
        if (progress < 1)
        {
            for (int i = 0; i < lightSources.Length; i++)
            {
                expiredTime += Time.deltaTime;
                progress = expiredTime / duration;
                lightSources[i].intensity = illuminationIntencivity.Evaluate(progress);
            }
            return;
        }
        progress = 0;
        expiredTime = 0;
    }
}
