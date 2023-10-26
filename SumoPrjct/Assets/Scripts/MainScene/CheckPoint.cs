using System;
using System.Collections;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform checkPoint;
    [SerializeField] private Animator checkPointAnimator;
    [SerializeField] private GameObject gameSavedMessage;
    [SerializeField] private GameObject lights;
    [SerializeField] private float animationLength;

    private string animatorParameter = "Enabled";

    public static Action PlayerSavedGame;


    private void OnEnable()
    {
        PlayerSavedGame += OnPlayerSavedGame;
    }
    private void OnDisable()
    {
        PlayerSavedGame -= OnPlayerSavedGame;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GlobalData.PlayerTag))
        {
            ActivateCheckPoint();

            PlayerSavedGame?.Invoke();
        }
    }
    private void ActivateCheckPoint()
    {
        GlobalData.currentCheckPoint = checkPoint;
        ActivateLights();
        checkPointAnimator.SetBool(animatorParameter, true);
        gameSavedMessage.SetActive(true);
    }    private void DiativateCheckPoint()
    {
        checkPointAnimator.SetBool(animatorParameter, false);
        StartCoroutine(DiactivateLights());
    }
    private void ActivateLights()
    {
        lights.SetActive(true);
    }
    private IEnumerator DiactivateLights()
    {
        yield return new WaitForSeconds(animationLength);
        lights.SetActive(false);
    }
    private void OnPlayerSavedGame()
    {
        if (GlobalData.currentCheckPoint != checkPoint)
        {
            DiativateCheckPoint();
        }
    }
}