using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sound_Player;
using System;

public enum PlayerAnimParameters
{
    Walking,
    StartJump,
    LandJump,
    Death,
    Attack,
    Throw,
    Hit,
    Interact,
    Falling
}
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
    [SerializeField] private SoundEffectPlayer soundEffectPlayer;
    private HealthControll healthControll;
    private Animator animator;
    private Rigidbody rb;
    private Explosion explosion;
    private PlayerActionTimer timer;

    //Переменные для хранения временных данных
    private float defaultMoveSpeed;
    private float defaultMaxSpeed;

    //events
    public static event Action PlayerInteracted;

    public bool isGrounded { get; private set; }
    public Vector3 moveVector { private get; set; }

    #region MonoBeh
    private void OnEnable()
    {
        Respawn.PlayerFell += OnPlayerFell;
        healthControll.DamageTaken += OnDamageTaken;
        healthControll.EntityDied += OnHeroDied;
        Buff.BuffCollected += OnBuffCollected;
        Enemy.EnemyAndPlayerContacted += OnEnemyColission;
        CheckPoint.PlayerSavedGame += OnPlayerSavedGame;
    }
    private void OnDisable()
    {
        Respawn.PlayerFell -= OnPlayerFell;
        healthControll.DamageTaken -= OnDamageTaken;
        healthControll.EntityDied -= OnHeroDied;
        Buff.BuffCollected -= OnBuffCollected;
        Enemy.EnemyAndPlayerContacted -= OnEnemyColission;
        CheckPoint.PlayerSavedGame -= OnPlayerSavedGame;
    }

    private void Awake()
    {
        healthControll = GetComponent<HealthControll>();
        explosion = GetComponent<Explosion>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        timer = GetComponent<PlayerActionTimer>();
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
        UpdateAnimations();
    }
    private void FixedUpdate()
    {
        SetCanExplode();
        Move();
    }

    private void UpdateAnimations()
    {
        if (moveVector.magnitude > 0)
            animator.SetBool(PlayerAnimParameters.Walking.ToString(), true);
        else
            animator.SetBool(PlayerAnimParameters.Walking.ToString(), false);
    }
    public void Move()
    {
        //вращение персонажа
        if (moveVector.magnitude > 0.1f)
        {
            float rotationAngle = Mathf.Atan2(moveVector.x, moveVector.z) * Mathf.Rad2Deg + cameraRoot.eulerAngles.y;
            Quaternion rotation = Quaternion.Euler(0f,rotationAngle,0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        maxSpeed = defaultMaxSpeed * moveVector.magnitude;
        Vector3 directionAlongSurface = surfaceSlider.Project(transform.forward);

        float currentVelocityXY = (Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z));
        if (currentVelocityXY < maxSpeed)
        {
            Vector3 direction = isGrounded ? directionAlongSurface : transform.forward;
            rb.AddForce(direction * moveVector.magnitude * moveSpeed, ForceMode.Impulse);//метод передвижения 
        }
    }
    public void StartInteractAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Interact"))
            return;
        if (!isGrounded)
            return;
        if (timer.ActionCanBePerformed) 
        {
            timer.OnActionPerformed();
            animator.SetTrigger(PlayerAnimParameters.Interact.ToString());
            PlayerInteracted?.Invoke();
        }
    }
    public void StartAttackAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;
        if (timer.ActionCanBePerformed)
        {
            timer.OnActionPerformed();
            animator.SetTrigger(PlayerAnimParameters.Attack.ToString());
        }
    }
    public void StartThrowAnimation()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Throw"))
            return;
        if (!isGrounded)
            return;
        if (timer.ActionCanBePerformed)
        {
            timer.OnActionPerformed();
            animator.SetTrigger(PlayerAnimParameters.Throw.ToString());
        }
    }
    public void StartJumpAnimation()
    {
        if (isGrounded)
        {
            animator.SetTrigger(PlayerAnimParameters.StartJump.ToString());
        }
        else
        {
            verticalAccelerator.SpeedUp();
        }
    }
    public void Jump()
    {
        animator.SetBool(PlayerAnimParameters.LandJump.ToString(), false);
        soundEffectPlayer.PlaySound(SoundName.Jump);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }
    public void Explode()
    {
        explosion.Explode(explosionDamage);
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
        if (!isGrounded)
            return;

        Vector3 dir;
        if (enemy == null)
        {
            dir = new Vector3(UnityEngine.Random.Range(-1f,1f),1, UnityEngine.Random.Range(-1f, 1f)).normalized;
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
        if (collision.gameObject.tag == "Ground")
        {
            if(collision.relativeVelocity.magnitude > 2f)
                soundEffectPlayer.PlaySound(SoundName.Landing);

            animator.SetBool(PlayerAnimParameters.Falling.ToString(), false);
            animator.SetBool(PlayerAnimParameters.LandJump.ToString(), true);
        }

        IsGroundedUpate(collision, true);
        if (/*isGrounded && */canExplode)
        {
            Explode();
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
        animator.SetBool(PlayerAnimParameters.Death.ToString(), true);
        soundEffectPlayer.PlaySound(SoundName.Die);
        LoseScreen.SetActive(true);
        CursorHider.ShowCursor();
        Destroy(healthControll);
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
    public void ResetAllAnimatorTriggers()
    {
        animator.ResetTrigger(PlayerAnimParameters.StartJump.ToString());
        animator.ResetTrigger(PlayerAnimParameters.Attack.ToString());
        animator.ResetTrigger(PlayerAnimParameters.Throw.ToString());
    }
    private void OnDamageTaken(Transform damager)
    {
        animator.SetTrigger(PlayerAnimParameters.Hit.ToString());
        soundEffectPlayer.PlaySound(SoundName.TakeDamage);
        JumpOnTakeDamage(damager);
    }
    private void OnHeroDied()
    {
        Die();
    }
    private void OnPlayerFell()
    {
        soundEffectPlayer.PlaySound(SoundName.Respawn);
    }   
    private void OnPlayerSavedGame()
    {
        if (healthControll.NotFull)
            healthControll.RestoreHealth();
    }
}