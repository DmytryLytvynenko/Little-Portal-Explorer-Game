using System;
using UnityEngine;

public class FrogDisabler : MonoBehaviour
{
    public static Action FrogArrivedHome;
    private FrogController currentFrogController;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<FrogController>(out FrogController frogController))
        {
            if (currentFrogController == frogController) return;

            currentFrogController = frogController;
            frogController.Disable();
            FrogArrivedHome?.Invoke();
        }
    }
}
