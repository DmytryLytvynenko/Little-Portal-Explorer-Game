using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float moveSpeed;
    public float yStop;// на какой высоте остановиться
    public float timeToLive;// Через сколько уничтожится
    public float timeGoDown;// Через сколько поедет вниз
    /*    public float timeToStop;// Через сколько остановится*/


    private bool goUp = true;
    private bool goDown = false;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Die", timeToLive);
        Invoke("ChangeMoveVector", timeGoDown);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y >= yStop)
            Stop();
        if (goUp)
            Move(Vector3.up);
        if (goDown)
            Move(-Vector3.up);
    }
   

    private void Die()
    {
        Destroy(this.gameObject);
    }

    private void Move(Vector3 moveVector)
    {
        //перемещение платформы
        
        transform.Translate(moveVector * moveSpeed * Time.fixedDeltaTime);
    }
    private void ChangeMoveVector()
    {
        goDown = true;
    }
    private void Stop()
    {
        goUp = false;
    }
}
