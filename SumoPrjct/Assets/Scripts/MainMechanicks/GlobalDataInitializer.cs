using UnityEngine;
using Utilities;

public class GlobalDataInitializer : MonoBehaviour
{
    private void Awake()
    {
        GlobalData.PlayerInstance = GameObject.FindGameObjectWithTag(GlobalData.PlayerTag);
    }
}
