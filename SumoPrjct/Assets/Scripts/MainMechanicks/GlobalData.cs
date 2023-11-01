using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    public static GameObject PlayerInstance;
    public static string PlayerTag = "Player";
    public static Transform CurrentCheckPoint;
    public static Vector3 DefaultCheckPoint = new Vector3(128f, 36f, 444f);
}
