using UnityEngine;

public class ThrowTutorialTracker : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            GetComponent<Animator>().SetBool("Disappear", true);
        }
    }
}