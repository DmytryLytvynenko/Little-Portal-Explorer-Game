using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] AnimationCurve moovementCurve;
    [SerializeField] protected float duration;
    [SerializeField] protected float height;
    [SerializeField] protected bool AtDownPosition = true;

    private float startHeight;

    private void Awake()
    {
        startHeight = transform.position.y;
    }
    private IEnumerator PlayAnimationUp()
    {
        float expiredTime = 0f;
        float progress = 0f;

        while (progress < 1)
        {
            expiredTime += Time.deltaTime;
            progress = expiredTime / duration;

            transform.position = new Vector3(transform.position.x, startHeight + moovementCurve.Evaluate(progress) * height, transform.position.z);

            yield return null;
        }
        AtDownPosition = false;
    }
    private IEnumerator PlayAnimationDown()
    {
        float expiredTime = duration;
        float progress = 1f;

        while (progress > 0)
        {
            expiredTime -= Time.deltaTime;
            progress = expiredTime / duration;

            transform.position = new Vector3(transform.position.x, startHeight + moovementCurve.Evaluate(progress) * height, transform.position.z);

            yield return null;
        }
        AtDownPosition = true;
    }
    public void ActivateElevator()
    {
        if(AtDownPosition)
            StartCoroutine(PlayAnimationUp());
        else
            StartCoroutine(PlayAnimationDown());
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
