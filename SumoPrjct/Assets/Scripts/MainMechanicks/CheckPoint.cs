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
    private bool Active = false;

    public static Action PlayerSavedGame;


    private void OnEnable()
    {
        checkPointAnimator.enabled = false;
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
        if (Active) return;

        checkPointAnimator.enabled = true;
        Active = true;
        GlobalData.CurrentCheckPoint = checkPoint;

        rememberCheckPoint(checkPoint);
        ActivateLights();
        checkPointAnimator.SetBool(animatorParameter, Active);
        gameSavedMessage.SetActive(true);
    }
    public void ActivateCheckPointOnLoad()
    {
        if (Active) return;

        checkPointAnimator.enabled = true;
        Active = true;
        GlobalData.CurrentCheckPoint = checkPoint;

        rememberCheckPoint(checkPoint);
        ActivateLights();
        checkPointAnimator.SetBool(animatorParameter, Active);
    }
    private void DiativateCheckPoint()
    {
        Active = false;

        checkPointAnimator.SetBool(animatorParameter, Active);
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
        checkPointAnimator.enabled = false;
    }
    private void rememberCheckPoint(Transform checkPointPosition)
    {
        PlayerPrefs.SetString("CheckPointPosition", $"{checkPointPosition.position.x}.{checkPointPosition.position.y}.{checkPointPosition.position.z}");
        PlayerPrefs.SetInt("CheckPointID", transform.GetInstanceID());
        print($"{checkPointPosition.position.x}.{checkPointPosition.position.y}.{checkPointPosition.position.z}");
    }
    private void OnPlayerSavedGame()
    {
        if (GlobalData.CurrentCheckPoint != checkPoint)
        {
            DiativateCheckPoint();
        }
    }
}