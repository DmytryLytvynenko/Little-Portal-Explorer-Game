using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    //Oсновные параметры
    [SerializeField] private float jumpForce;
    [SerializeField] private float moveSpeed;
/*    [SerializeField] private float maxSpeed;*/
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool canExplode = false;
    [SerializeField] private float ExplosionJumpHeight;
    [SerializeField] private float velocityToExplode;
    [SerializeField] private float damageJumpForce;
    [SerializeField] private int explosionDamage;


    //Ссылки на компоненты
    [SerializeField] private GameObject LoseScreen;
    private MobileController mController;
    private Animator ch_animator;
    private Rigidbody rb;
    private Explosion explosion;

    private Vector3 moveVector// направление  передвижения
    {
        get
        {
            var horizontal = mController.Horizontal();
            var vertical = mController.Vertical();

            return new Vector3(horizontal, 0.0f, vertical);
        }
    }

    private void Start()
    {
        explosion = GetComponent<Explosion>();
        rb = GetComponent<Rigidbody>();
        /*ch_animator = GetComponent<Animator>();*/
        mController = GameObject.FindGameObjectsWithTag("Joystick")[0].GetComponent<MobileController>();
    }

    private void Update()
    {
        SetCanExplode();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        Move();
    }

    private void Move()
    {
        //вращение персонажа
        if (moveVector.magnitude > 0.1f)
        {
            Quaternion rotation = Quaternion.LookRotation(moveVector);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        //перемещение персонажа
        // без инерции
        Vector3 offset = moveVector * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + offset);

        // с инерцией
        /*        if (rb.velocity.magnitude < maxSpeed)
                {
                    rb.AddForce(moveVector * moveSpeed, ForceMode.Impulse);//метод передвижения 
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
/*    public void Throw()
    {
        Collider[] overlappedColiders = Physics.OverlapBox(throwArea.position, new Vector3(xThrowSize/2, yThrowSize/2, zThrowSize/2), throwArea.rotation);
        for (int i = 0; i < overlappedColiders.Length; i++)
        {
            Rigidbody rigitbody = overlappedColiders[i].attachedRigidbody;
            if (rigitbody && !rigitbody.gameObject.CompareTag("Player"))
            {
                if (rigitbody.gameObject.CompareTag("Bomb"))
                {
                    rigitbody.gameObject.GetComponent<Bomb>().SetActive(true);
                }
                Vector3 enemyDir = new Vector3(rigitbody.transform.position.x - transform.position.x, (rigitbody.transform.position.y - transform.position.y) + yThrowAngle, rigitbody.transform.position.z - transform.position.z);
                Debug.DrawLine(transform.position, new Vector3(rigitbody.transform.position.x, rigitbody.transform.position.y + yThrowAngle, rigitbody.transform.position.z), Color.red, 100);
                rigitbody.AddForce(enemyDir.normalized * throwForce, ForceMode.Impulse);
                Debug.Log("Throw!");
                if (!rigitbody.gameObject.GetComponent<HealthControll>())
                {
                    continue;
                }
                else
                {
                    rigitbody.GetComponent<HealthControll>().ChangeHealth(-throwDamage);
                }
            }
        }
    }*/
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
        if (collision.gameObject.CompareTag("Ground") && canExplode)
        {
            explosion.Explode(explosionDamage);
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
    private void OnDrawGizmos()
    {

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
}