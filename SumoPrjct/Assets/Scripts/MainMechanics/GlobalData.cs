using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    public static HeroController PlayerInstance;
    public static CoinPool coinPool;
    public static HealPool healPool;
    public static string PlayerTag = "Player";
    public static Transform CurrentCheckPoint;
    public static Vector3 DefaultCheckPoint = new Vector3(128f, 36f, 444f);
}