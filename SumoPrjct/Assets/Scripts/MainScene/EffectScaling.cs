using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectScaling : MonoBehaviour
{
    public void SetScale(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).localScale = new Vector3(scale, scale, scale);
            }
        }
    }
}
