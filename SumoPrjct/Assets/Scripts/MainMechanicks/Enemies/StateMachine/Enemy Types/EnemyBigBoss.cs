using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBigBoss : Enemy
{
    // Main characteristics
    [Header("Main")]
    [SerializeField] private float runSpeedIncrement;
    [SerializeField] private int explosionDamage;
    [SerializeField] private GameObject explosionPointer;
    private GameObject _explosionPointer;
    private Throw _throw;
    private Explosion _explosion;
    private Action currentAction;
    private float tempMoveSpeed;  

    [Header("Shoot")]
    [SerializeField] private float waitTillShoot;
    [SerializeField] private float shootDistnace;
    [SerializeField] private float xShootAngle;
    [SerializeField] private float numberOfBullets;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private GameObject bullet;
    private Transform ShootPos; // откуда стреляем


    [Header("Jump")]
    [SerializeField] private float jumpDistnace;
    private bool canJump;
    [SerializeField] private float waitTilljump;
    [SerializeField] private float jumpHeight = 7;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private bool debugPath;


    [Header("Kick")]
    [SerializeField] private float kickDistnace;
    [SerializeField] private float waitTillKick;

    private bool isBusy;
    private enum Action
    {
        Shoot,
        Jump,
        Kick,
        None
    }
    protected override void Awake()
    {
        base.Awake();

        tempMoveSpeed = moveSpeed;
        _explosion = GetComponent<Explosion>();
        _throw = GetComponent<Throw>();
        rb = GetComponent<Rigidbody>();
        Target = GameObject.Find("Hero").GetComponent<Transform>();
        ShootPos = this.gameObject.transform.GetChild(0);
        currentAction = Action.None;
    }
    protected override void Update()
    {
        base.Update();
        Rotate(Target);
        Move();
        ControllDistance();
    }
    public override void Move()
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            Vector3 offset = transform.forward * moveSpeed * Time.deltaTime;
            rb.MovePosition(rb.position + offset);//метод передвижения 
        }
    }
    private void ChooseAction()
    {
        if (!isGrounded)
        {
            return;
        }
        if (currentAction != Action.None)
        {
            //Если выбранное действие не сменялось долго, то оно сбрасывается(currentAction = Action.None) и в следующий раз выберется другое
/*            if (chooseActionTimer >= chooseActionTime)
            {
                currentAction = Action.None;
            }
            chooseActionTimer += Time.deltaTime;*/
            return;
        }
        int randomDigit = Random.Range(1,4);
        switch (randomDigit)
        {
            case (1):
                currentAction = Action.Jump;
                break;
            case (2):
                currentAction = Action.Shoot;
                break;
            case (3):
                currentAction = Action.Kick;
                moveSpeed += runSpeedIncrement;
                maxSpeed += runSpeedIncrement;
                break;
        }
        IndicateAction(currentAction);
    }
    
    private void ControllDistance()
    {
        if (!isGrounded)
            return;

        if (currentAction == Action.None)
        {
            DeIndicateAction();
            ChooseAction();
        }
        if (isBusy)
        { 
            return;
        }

        if (CompareDistance(currentAction))
        {
            switch (currentAction)
            {
                case Action.Jump:
                    StartCoroutine(Jump());
                    break;
                case Action.Kick:
                    StartCoroutine(Kick());
                    break;
                case Action.Shoot:
                    StartCoroutine(Shoot());
                    break;
            }
        }
    }
    public IEnumerator Kick()
    {
        isBusy = true;
        moveSpeed = 0;
        yield return new WaitForSeconds(waitTillKick);
        _throw.EnemyBossThrow();
        yield return new WaitForSeconds(waitTillKick);
        moveSpeed = tempMoveSpeed;
        maxSpeed -= runSpeedIncrement;
        currentAction = Action.None;
        isBusy = false;
    }
    private IEnumerator Jump()
    {
        isBusy = true;
        float tempRotSpeed = rotationSpeed;
        moveSpeed = 0;
        rotationSpeed = 0;
        yield return new WaitForSeconds(waitTilljump);

        DrawPointer();
        Launch();
        moveSpeed = tempMoveSpeed;
        rotationSpeed = tempRotSpeed;
        currentAction = Action.None;
        isBusy = false;
    }
    private IEnumerator Shoot()
    {
        isBusy = true;
        moveSpeed = 0;
        yield return new WaitForSeconds(waitTillShoot);

        for (int i = 0; i < numberOfBullets; i++)
        {
            GameObject bullet = Instantiate(this.bullet, ShootPos.position, Quaternion.Euler(0f, transform.localEulerAngles.y, transform.localEulerAngles.z));
            bullet.GetComponent<Bullet>().targetPoint = Target;

            yield return new WaitForSeconds(timeBetweenShots);
        }
        moveSpeed = tempMoveSpeed;
        currentAction = Action.None;
        isBusy = false;
    }
    private void IndicateAction(Action action)
    {
        switch (action)
        {
            case Action.Shoot:
                GetComponent<Renderer>().material.color = Color.blue;
                break;

            case Action.Jump:
                GetComponent<Renderer>().material.color = Color.yellow;
                break;

            case Action.Kick:
                GetComponent<Renderer>().material.color = Color.green;
                break;

            default:
                break;
        }
    }
    private void DeIndicateAction()
    {
        GetComponent<Renderer>().material.color = Color.red;
    }
    private bool CompareDistance(Action currentAction)
    {
        float distance = 0f;
        switch (currentAction)
        {
            case Action.Shoot:
                distance = shootDistnace;
                break;
            case Action.Jump:
                distance = jumpDistnace;
                break;
            case Action.Kick:
                distance = kickDistnace;
                break;
        }
        if (GetRotationVector(Target).magnitude <= distance)
            return true;
        else
            return false;
    }
    private void DrawPointer()
    {
        Ray ray = new Ray(Target.position, -Vector3.up * 10);
        Physics.Raycast(ray, out RaycastHit hit);
        Vector3 drawPoint = hit.point;
        _explosionPointer = Instantiate(explosionPointer, drawPoint, Quaternion.identity);
    }
    private void Launch()
    {
        rb.velocity = CalculateLaunchData().initialVelocity;
    }
    LaunchData CalculateLaunchData()
    {
        if (Target.position.y >= rb.position.y)
        {
            jumpHeight = Target.position.y + 1;
        }
        else
        {
            jumpHeight = rb.position.y + 1;
        }

        float displacementY = Target.position.y - rb.position.y;
        Vector3 displacementXZ = new Vector3(Target.position.x - rb.position.x, 0, Target.position.z - rb.position.z);
        float time = Mathf.Sqrt(-2 * jumpHeight / gravity) + Mathf.Sqrt(2 * (displacementY - jumpHeight) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * jumpHeight);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }
    struct LaunchData
    {
        public readonly Vector3 initialVelocity;
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }

    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (!isGrounded)
        {
            _explosion.BossExplode(explosionDamage);
        }
        if (_explosionPointer != null)
        {
            Destroy(_explosionPointer);
        }
    }
}

