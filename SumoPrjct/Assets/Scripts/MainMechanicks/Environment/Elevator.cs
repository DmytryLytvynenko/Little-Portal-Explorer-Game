using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] AnimationCurve moovementCurve;
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    [SerializeField] protected float duration;
    [SerializeField] protected float height;
    [SerializeField] protected bool AtStartPosition = true;

    private Vector3 moveVector;
    private Vector3 startPosition;
    private IEnumerator PlayAnimation()
    {
        if (AtStartPosition)
        {
            moveVector = endPoint.position - startPoint.position;
        }
        else
        {
            moveVector = startPoint.position - endPoint.position;
        }
        startPosition = transform.position;
        float expiredTime = 0f;
        float progress = 0f;

        while (progress < 1)
        {
            expiredTime += Time.deltaTime;
            progress = expiredTime / duration;

            transform.position = startPosition + moveVector * moovementCurve.Evaluate(progress);

            yield return null;
        }
        AtStartPosition = !AtStartPosition;
    }
    public void ActivateElevator()
    {
        StartCoroutine(PlayAnimation());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<HeroController>())
        {
            other.transform.parent = transform;
            ActivateElevator();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}