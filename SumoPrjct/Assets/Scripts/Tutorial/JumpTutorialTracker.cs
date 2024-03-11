using UnityEngine;

public class JumpTutorialTracker : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetBool("Disappear", true);
        }
    }
}