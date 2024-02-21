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

    protected Rigidbody rb;
    protected private Coroutine coroutine;

    private Vector3 moveVector;
    private Vector3 startPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private IEnumerator PlayAnimation()
    {
        if (AtStartPosition)
        {
            moveVector = (endPoint.position - startPoint.position);
        }
        else
        {
            moveVector = (startPoint.position - endPoint.position);
        }
        startPosition = transform.position;
        float expiredTime = 0f;
        float progress = 0f;

        while (progress < 1)
        {
            expiredTime += Time.deltaTime;
            progress = expiredTime / duration;

            /*            Vector3 offset = new Vector3(moveVector.x * expiredTime, moveVector.y * expiredTime, moveVector.z * expiredTime);
                        rb.MovePosition(rb.position + offset);*/
            /*            transform.position = startPosition + moveVector * moovementCurve.Evaluate(progress);*/
            Vector3 dir = (startPosition + moveVector * moovementCurve.Evaluate(progress) - transform.position);
            transform.Translate(dir);

            yield return null;
        }
        AtStartPosition = !AtStartPosition;
        coroutine = null;
    }
    public void ActivateElevator()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(PlayAnimation());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        HeroController heroController;
        if (other.TryGetComponent(out heroController))
        {
            heroController.transform.parent = transform;
            ActivateElevator();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Enemy enemyController;
        HeroController heroController;
        if (other.TryGetComponent(out heroController))
        {
            other.transform.parent = null;
            return;
        }
        if (other.TryGetComponent(out enemyController))
        {
            other.transform.parent = null;
            return;
        }
    }
}