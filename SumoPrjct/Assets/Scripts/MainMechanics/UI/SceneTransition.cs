using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    private static SceneTransition instance;
    private static bool  shouldPlayOpeningAnimation = true;

    private Animator animator;
    private AsyncOperation loadingOperation;


    private void Start()
    {
        instance = this;

        animator = GetComponent<Animator>();

        if (shouldPlayOpeningAnimation) animator.SetTrigger("SceneOpening");
    }
    public static void SwitchToScene(string sceneName)
    {
        instance.animator.SetTrigger("SceneClosing");

        instance.loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        Time.timeScale = 1.0f;
        instance.loadingOperation.allowSceneActivation = false;
    }

    public void OnAnimationOver()
    {
        shouldPlayOpeningAnimation = true;
        loadingOperation.allowSceneActivation = true;
    }
}
