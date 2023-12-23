using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sound_Player;
public class HeroController : MonoBehaviour
{
    [Header("Oсновные параметры")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float ExplosionJumpHeight;
    [SerializeField] private float velocityToExplode;
    [SerializeField] private int explosionDamage;
    [SerializeField] private LayerMask raycastLayerMask;
    [SerializeField] private float damageJumpForce;

    private bool canExplode = false;

    //Elements for buff system
    Dictionary<string, MyDelegate> buffableStats;
    private delegate IEnumerator MyDelegate(float newRadius, float buffCooldown);


    [Header("Cсылки на компоненты")]
    [SerializeField] private GameObject LoseScreen;
    [SerializeField] private Transform cameraRoot;
    [SerializeField] private SurfaceSlider surfaceSlider;
    [SerializeField] private VerticalAccelerator verticalAccelerator;
    [SerializeField] private SoundPlayer soundPlayer;
    [field:SerializeField] public PlayerDeformator PlayerDeformator { get; private set; }
    private HealthControll healthControll;
    private MobileController mController;
    private Animator ch_animator;
    private Rigidbody rb;
    private Explosion explosion;

    //Переменные для хранения временных данных
    private float defaultMoveSpeed;
    private float defaultMaxSpeed;

    public bool isGrounded { get; private set; }
    private Vector3 moveVector// направление  передвижения
    {
        get
        {
            var horizontal = mController.Horizontal();
            var vertical = mController.Vertical();

            return new Vector3(horizontal, 0.0f, vertical);
        }
    }

    #region MonoBeh
    private void OnEnable()
    {
        Respawn.PlayerFell += OnPlayerFell;
        healthControll.DamageTaken += OnDamageTaken;
        Buff.BuffCollected += OnBuffCollected;
        Enemy.EnemyAndPlayerContacted += OnEnemyColission;
    }
    private void OnDisable()
    {
        Respawn.PlayerFell -= OnPlayerFell;
        healthControll.DamageTaken -= OnDamageTaken;
        Buff.BuffCollected -= OnBuffCollected;
        Enemy.EnemyAndPlayerContacted -= OnEnemyColission;
    }

    private void Awake()
    {
        healthControll = GetComponent<HealthControll>();
        explosion = GetComponent<Explosion>();
        rb = GetComponent<Rigidbody>();
        /*ch_animator = GetComponent<Animator>();*/
        mController = GameObject.FindGameObjectWithTag("Joystick").GetComponent<MobileController>();

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
        defaultMoveSpeed = moveSpeed;
        defaultMaxSpeed = maxSpeed;
    } 
    #endregion

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
        Vector3 directionAlongSurface = surfaceSlider.Project(transform.forward);

        float currentVelocityXY = (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z));
        if (currentVelocityXY < maxSpeed)
        {
            Vector3 direction = isGrounded ? directionAlongSurface : transform.forward;
            rb.AddForce(direction * moveVector.magnitude * moveSpeed, ForceMode.Impulse);//метод передвижения 
        }
    }
    public void Jump()
    {
        if (isGrounded)
        {
            soundPlayer.PlaySound(SoundName.Jump);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
        else
        {
            verticalAccelerator.SpeedUp();
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
        Physics.Raycast(ray, out hit, 1000f, raycastLayerMask);
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
        Vector3 dir;
        if (enemy == null)
        {
            dir = new Vector3(Random.Range(-1f,1f),1, Random.Range(-1f, 1f)).normalized;
        }
        else
        {
            dir = new Vector3(
                transform.position.x - enemy.position.x,
                (transform.position.y + 2f - enemy.position.y),
                transform.position.z - enemy.position.z).normalized;
        }
        rb.AddForce(dir * damageJumpForce, ForceMode.Impulse);
        explosion.NoDamageExplode();

    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == ("Ground"))
            soundPlayer.PlaySound(SoundName.Landing);

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
    private void OnEnemyColission(int contactDamage, Transform damager)
    {
        healthControll.ChangeHealth(contactDamage, damager);
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
        LoseScreen.SetActive(true);
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
    public void DeactivateDeformation()
    {
        PlayerDeformator.transform.parent = transform;
        PlayerDeformator.gameObject.SetActive(false);
    }
    public void ActivateDeformation()
    {
        PlayerDeformator.transform.parent = null;
        PlayerDeformator.gameObject.SetActive(true);
    }
    private void OnDamageTaken(Transform damager)
    {
        soundPlayer.PlaySound(SoundName.TakeDamage);
        JumpOnTakeDamage(damager);
    }
    private void OnPlayerFell()
    {
        soundPlayer.PlaySound(SoundName.Respawn);
    }
}