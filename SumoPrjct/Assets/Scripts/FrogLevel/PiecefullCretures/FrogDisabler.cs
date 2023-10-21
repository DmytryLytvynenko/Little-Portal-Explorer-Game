using System;
using UnityEngine;

public class FrogDisabler : MonoBehaviour
{
    public static Action FrogArrivedHome;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<FrogController>(out FrogController frogController))
        {
            frogController.Disable();
            FrogArrivedHome?.Invoke();
            frogController.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
}
