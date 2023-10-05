using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HeroController : MonoBehaviour
{
    //Oсновные параметры
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float ExplosionJumpHeight;
    [SerializeField] private float velocityToExplode;
    [SerializeField] private int explosionDamage;
    private bool canExplode = false;
    private float damageJumpForce;
    Dictionary<string, MyDelegate> buffableStats;
    private delegate IEnumerator MyDelegate(float newRadius);
    [SerializeField] private bool isGrounded;


    //Ссылки на компоненты
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private Transform cameraRoot;
    private MobileController mController;
    private Animator ch_animator;
    private Rigidbody rb;
    private Explosion explosion;

    //Переменные для хранения временных данных
    private float defaultMoveSpeed;
    private float defaultMaxSpeed;

    private Vector3 moveVector// направление  передвижения
    {
        get
        {
            var horizontal = mController.Horizontal();
            var vertical = mController.Vertical();

            return new Vector3(horizontal, 0.0f, vertical);
        }
    }
    private void OnEnable()
    {
        Buff.BuffCollected += OnBuffCollected;
    }
    private void OnDisable()
    {
        Buff.BuffCollected -= OnBuffCollected;
    }

    private void Start()
    {
        MyDelegate[] buffFoos = new MyDelegate[5];//magic number

        // Присваиваем значения элементам массива
        buffFoos[0] = BuffExplosionRadius;

        buffableStats = new Dictionary<string, MyDelegate>() 
        {
            { "Explosion Radius", buffFoos[0]}//magic word!!!
        };
        defaultMoveSpeed = moveSpeed;
        defaultMaxSpeed = maxSpeed;
        damageJumpForce = jumpForce;
        explosion = GetComponent<Explosion>();
        rb = GetComponent<Rigidbody>();
        /*ch_animator = GetComponent<Animator>();*/
        mController = GameObject.FindGameObjectsWithTag("Joystick")[0].GetComponent<MobileController>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    private void FixedUpdate()
    {
        SetCanExplode();
        Move();
    }

    private void Move()
    {
        //вращение персонажа
        if (moveVector.magnitude > 0.1f)
        {
            float rotationAngle = Mathf.Atan2(moveVector.x,moveVector.z) * Mathf.Rad2Deg + cameraRoot.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f,rotationAngle,0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        //перемещение персонажа

        // без инерции
        /*        Vector3 offset = transform.forward * moveVector.magnitude * moveSpeed * Time.deltaTime;
                rb.MovePosition(rb.position + offset);*/

        // с инерцией
        maxSpeed = defaultMaxSpeed * moveVector.magnitude;

        float currentVelocityXY = (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z));
        if (currentVelocityXY < maxSpeed)
        {
            rb.AddForce(transform.forward * moveVector.magnitude * moveSpeed, ForceMode.Impulse);//метод передвижения 
        }
    }
    public void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }
    private void SetCanExplode()
    {
        if (isGrounded)
        {
            return;
        }

        Ray ray = new Ray(transform.position, -100*Vector3.up);
        Debug.DrawRay(transform.position, -100 * Vector3.up, Color.red);
        RaycastHit hit;
        Physics.Raycast(ray, out hit);
        if (!hit.collider)
        {
            return;
        }
        if (hit.collider.isTrigger)
        {
            return;
        }
        if (hit.distance >= ExplosionJumpHeight && rb.velocity.y < velocityToExplode)
        {
            canExplode = true;
        }
    }
    private void JumpOnTakeDamage(Transform enemy)
    {
        Vector3 dir = new Vector3(enemy.transform.position.x - transform.position.x, 0, enemy.transform.position.z - transform.position.z);
        rb.AddForce(-dir * damageJumpForce, ForceMode.Impulse);
        explosion.NoDamageExplode();
        Debug.DrawLine(enemy.transform.position, transform.position, Color.green, 100f);
    }
    void OnCollisionEnter(Collision collision)
    {
        IsGroundedUpate(collision, true);
        if (isGrounded && canExplode)
        {
            explosion.Explode(explosionDamage);
            canExplode = false;
        }
        else
        {
            canExplode = false;
        }
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Bullet") || collision.gameObject.CompareTag("Boss"))
        {
            JumpOnTakeDamage(collision.transform);
        }
    }
    void OnCollisionExit(Collision collision)
    {
        IsGroundedUpate(collision, false);
    }
    private void OnCollisionStay(Collision collision)
    {
        IsGroundedUpate(collision, true);
    }
    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground") || collision.gameObject.tag == ("Platform"))
        {
            isGrounded = value;
        }
    }

    private void OnBuffCollected(float newValue, string buffedStatName)
    {
        buffableStats.TryGetValue(buffedStatName, out MyDelegate foo);
        StartCoroutine(foo(newValue));
    }
    private IEnumerator BuffExplosionRadius(float newRadius)
    {
        explosion.SetExplosionRadius(newRadius);
        yield return new WaitForSeconds(7);// magic number!!!
        print("radius normal again");
        explosion.SetExplosionRadiusToNormal();
    }
    private IEnumerator WaitBuffToEnd(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
    }
    public void Die()
    {
        GameObject.Find("JumpButton").GetComponent<Button>().interactable = false;
        GameObject.Find("ThrowButton").GetComponent<Button>().interactable = false;
        GameObject.Find("ExplosionButton").GetComponent<Button>().interactable = false;
        LoseScreen.SetActive(true);
        Time.timeScale = 0;
    }
    public void Win()
    {
        GameObject.Find("JumpButton").GetComponent<Button>().interactable = false;
        GameObject.Find("ThrowButton").GetComponent<Button>().interactable = false;
        GameObject.Find("ExplosionButton").GetComponent<Button>().interactable = false;
        gameObject.GetComponent<HeroController>().enabled = false;
        Time.timeScale = 0;
    }
    public void SetSpeedToZero()
    {
        defaultMoveSpeed = moveSpeed;
        moveSpeed = 0;
    }
    public void SetSpeedToDefault()
    {
        moveSpeed = defaultMoveSpeed;
    }

}