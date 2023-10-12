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
    [SerializeField] private bool isGrounded;

    //Elements for buff system
    Dictionary<string, MyDelegate> buffableStats;
    private delegate IEnumerator MyDelegate(float newRadius, float buffCooldown);


    //Ссылки на компоненты
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private Transform cameraRoot;
    private HealthControll healthControll;
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
        Entity.EnemyAndPlayerContacted += OnEnemyColission;
    }
    private void OnDisable()
    {
        Buff.BuffCollected -= OnBuffCollected;
        Entity.EnemyAndPlayerContacted -= OnEnemyColission;
    }

    private void Awake()
    {
        MyDelegate[] buffFoos = new MyDelegate[]
        {
            // Присваиваем значения элементам массива
            BuffExplosionRadius,
            BuffMaxSpeed,
            BuffJumpForce
        };

        buffableStats = new Dictionary<string, MyDelegate>() 
        {
            // Присваиваем значения элементам словаря <название усиленной характеристики, функция которая ее усиливает>
            { "Explosion Radius", buffFoos[0]},//magic word!!!
            { "Max Speed", buffFoos[1]},//magic word!!!
            { "Jump Force", buffFoos[2]}//magic word!!!
        };
    }
    private void Start()
    {
        healthControll = GetComponent<HealthControll>();
        explosion = GetComponent<Explosion>();
        rb = GetComponent<Rigidbody>();
        /*ch_animator = GetComponent<Animator>();*/
        mController = GameObject.FindGameObjectWithTag("Joystick").GetComponent<MobileController>();

        defaultMoveSpeed = moveSpeed;
        defaultMaxSpeed = maxSpeed;
        damageJumpForce = jumpForce;
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
        }/*
        if (!isGrounded)
        {
            if (currentVelocityXY >= maxSpeed)
            {
                rb.AddForce(-transform.forward * moveVector.magnitude * moveSpeed/2, ForceMode.Force); 
                rb.AddForce(transform.forward * moveVector.magnitude * moveSpeed, ForceMode.Force);
            }
            else
            {
                rb.AddForce(transform.forward * moveVector.magnitude * moveSpeed, ForceMode.Force);
            }
        }*/
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

    private void OnBuffCollected(float newValue, float buffCooldown, string buffedStatName)
    {
        buffableStats.TryGetValue(buffedStatName, out MyDelegate foo);
        StartCoroutine(foo(newValue, buffCooldown));
    }   
    private void OnEnemyColission(int contactDamage)
    {
        healthControll.ChangeHealth(contactDamage);
    }
    private IEnumerator BuffExplosionRadius(float newRadius, float buffCooldown)
    {
        explosion.SetExplosionRadius(newRadius);
        yield return new WaitForSeconds(buffCooldown);
        print("radius normal again");
        explosion.SetExplosionRadiusToNormal();
    }
    private IEnumerator BuffMaxSpeed(float newMaxSpeed, float buffCooldown)
    {
        float increment = newMaxSpeed - defaultMaxSpeed;
        defaultMaxSpeed += increment;
        print($"Max speed now {defaultMaxSpeed}");

        yield return new WaitForSeconds(buffCooldown);

        defaultMaxSpeed -= increment;
        print($"Max speed normal again ({defaultMaxSpeed})");
    }
    private IEnumerator BuffJumpForce(float newJumpForce, float buffCooldown)
    {
        float increment = newJumpForce - jumpForce;
        jumpForce += increment;
        print($"jumpForce now {jumpForce}");

        yield return new WaitForSeconds(buffCooldown);

        jumpForce -= increment;
        print($"jumpForce normal again ({jumpForce})");
    }   
    public void Die()
    {
        // это надо оптимизировать
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