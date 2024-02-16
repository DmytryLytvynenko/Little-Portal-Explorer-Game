using Sound_Player;
using System;
using System.Collections;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform checkPoint;
    [SerializeField] private Animator checkPointAnimator;
    [SerializeField] private GameObject gameSavedMessage;
    [SerializeField] private GameObject lights;
    [SerializeField] private SoundEffectPlayer soundEffectPlayer;
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
        if (other.GetComponent<HeroController>())
        {
            ActivateCheckPoint();

            PlayerSavedGame?.Invoke();
        }
    }
    private void ActivateCheckPoint()
    {
        if (Active) return;
        soundEffectPlayer.PlaySound(SoundName.GameSaved);

        checkPointAnimator.enabled = true;
        Active = true;
        GlobalData.CurrentCheckPoint = checkPoint;

        RememberCheckPoint(checkPoint);
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

        RememberCheckPoint(checkPoint);
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
    private void RememberCheckPoint(Transform checkPointPosition)
    {
        PlayerPrefs.SetString("CheckPointPosition", $"{checkPointPosition.position.x}.{checkPointPosition.position.y}.{checkPointPosition.position.z}");
        PlayerPrefs.SetString("CheckPointName", transform.name);
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