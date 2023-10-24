using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDataInitializer : MonoBehaviour
{
    void Start()
    {
        GlobalData.PlayerInstance = GameObject.FindGameObjectWithTag(GlobalData.PlayerTag);
       
    }
}
