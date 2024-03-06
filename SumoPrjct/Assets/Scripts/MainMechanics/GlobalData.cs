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
    public static Vector3 DefaultCheckPoint = new Vector3(144.7f, 36f, 457.27f);
}