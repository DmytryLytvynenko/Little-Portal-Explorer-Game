using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Coroutines : MonoBehaviour
{
    private static Coroutines instance 
    {
        get 
        {
            if (_instance == null)
            {
                var gameObject = new GameObject("COROTINE MANAGER");
                _instance = gameObject.AddComponent<Coroutines>();
                DontDestroyOnLoad(gameObject);
            }
            return _instance;
        }
    }
    private static Coroutines _instance;

    public static Coroutine StartRoutine(IEnumerator enumerator)
    {
        return instance.StartCoroutine(enumerator);
    }
    public static void StopRoutine(Coroutine coroutine)
    {
        if (coroutine != null) 
        instance.StopCoroutine(coroutine);
    }
}
