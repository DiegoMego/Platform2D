using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroController : MonoBehaviour
{
    private float speed = 7f;
    private float jumpSpeed = 7f;
    private float extraSpace = 0.2f;
    private float fallMultiplier = 2;
    private float loadJumpMultiplier = 2;
    private bool canDoubleJump = false;
    private float startfallingthreshold = -3f;
    private float teleportationDistance = 18f;
    private float minimunStartPosition = -19f;
    public GameObject bulletPrefab;

    private Animator animator;
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private float colliderHeight;
    private bool isJumping = false;
    private bool isAlive = true;
    private Vector3 startPosition;
    private Transform firePoint;
    private Slider powerBarSlider;
    private Slider healthBarSlider;
    private float enemyContactTime = 0f;
    private float contactTimeToRecieveDamage = 3f;

    private void Awake()
    {
        firePoint = transform.Find("FirePoint");
        powerBarSlider = transform.Find("Canvas").Find("PowerBar").GetComponent<Slider>();
        healthBarSlider = transform.Find("Canvas").Find("HealthBar").GetComponent<Slider>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        colliderHeight = capsuleCollider.bounds.extents.y;
        startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        if (isAlive)
        {
            isJumping = !IsNotJumping();

            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Physics2D.gravity * (fallMultiplier - 1) * Time.fixedDeltaTime;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.velocity += Vector2.up * Physics2D.gravity * (loadJumpMultiplier - 1) * Time.fixedDeltaTime;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (isAlive)
        {
            if (isJumping)
            {
                if (rb.velocity.y > 0f)
                {
                    animator.SetBool("IsFalling", false);
                    animator.SetBool("IsJumping", true);
                }
                else if (rb.velocity.y < startfallingthreshold)
                {
                    animator.SetBool("IsJumping", false);
                    animator.SetBool("IsFalling", true);
                }
            }
            else
            {
                animator.SetBool("IsJumping", false);
                animator.SetBool("IsFalling", false);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                Fire();
            }

            var movement = Input.GetAxisRaw("Horizontal");

            if (movement != 0f)
            {
                animator.SetBool("IsRunning", true);
                if (movement < 0)
                {
                    if (transform.localScale.x != -1f)
                    {
                        transform.localScale = new Vector3(
                            -1f,
                            transform.localScale.y,
                            transform.localScale.z
                        );
                    }
                }
                else
                {
                    if (transform.localScale.x != 1f)
                    {
                        transform.localScale = new Vector3(
                            1f,
                            transform.localScale.y,
                            transform.localScale.z
                        );
                    }
                }
            }
            else
            {
                animator.SetBool("IsRunning", false);
            }

            transform.position += Vector3.right * movement * speed * Time.deltaTime;

            //Left invisible wall so hero doesn't jump off running to the left
            if (transform.position.x <= minimunStartPosition)
            {
                transform.position = new Vector3(
                    minimunStartPosition,
                    transform.position.y,
                    transform.position.z
                );
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (!isJumping)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);
                    canDoubleJump = true;
                }
                else if (canDoubleJump)
                {
                    canDoubleJump = false;
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(transform.up * jumpSpeed, ForceMode2D.Impulse);
                }
            }

            if (Input.GetKeyDown(KeyCode.F))
            {
                if (powerBarSlider.value == powerBarSlider.maxValue)
                {
                    Teleport();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.position = startPosition;
                rb.bodyType = RigidbodyType2D.Dynamic;
                healthBarSlider.value = healthBarSlider.maxValue;
                powerBarSlider.value = 0f;
                isAlive = true;
            }
        }
    }

    private void Teleport()
    {
        if (transform.localScale.x == -1)
        {
            transform.position = new Vector3(
                transform.position.x - teleportationDistance,
                transform.position.y,
                transform.position.z
            );
        }
        else
        {
            transform.position = new Vector3(
                transform.position.x + teleportationDistance,
                transform.position.y,
                transform.position.z
            );
        }
        powerBarSlider.value = 0f;
    }

    private bool IsNotJumping()
    {
        RaycastHit2D hit = Physics2D.Raycast(
            capsuleCollider.bounds.center,
            Vector2.down,
            colliderHeight + extraSpace
        );
        //Color rayColor = Color.yellow;
        return hit;
    }

    private void Fire()
    {
        animator.SetTrigger("Fire");
        if (transform.localScale.x == -1)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            bullet.transform.Rotate(0f, 0f, 180f);
        }
        else
        {
            Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        }
    }

    private void Hurt(float damage)
    {
        animator.SetTrigger("IsHurt");
        healthBarSlider.value -= damage;
        if (healthBarSlider.value <= 0)
        {
            this.isAlive = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            enemyContactTime = Time.time;
            Hurt(healthBarSlider.maxValue * 0.2f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            if (Time.time - enemyContactTime >= contactTimeToRecieveDamage)
            {
                Hurt(healthBarSlider.maxValue * 0.2f);
                enemyContactTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Water"))
        {
            isAlive = false;
            transform.position += Vector3.down * 2f;
            rb.bodyType = RigidbodyType2D.Static;
        }
        if (collision.transform.CompareTag("FireBreath"))
        {
            Hurt(healthBarSlider.maxValue * 0.25f);
            enemyContactTime = Time.time;
        }
    }

    private void TODO()
    {
        /*
         mecanica doble salto:
        - verificar que salto una vez
            --contador
            --reseteo a caer al piso (revisar con colider)
        - que no ocurra una tercera, cuarta...
        --------------------------------------------------------
        textbox de barra de poder (al inicio vacia)
        constante para teletransporte fijo
        vector de velocidad (+ o -)
        al presionar (f), debe revisar la barra de poder
        cuando la bala choque con el ememigo, debe aumentar la barra (4 de prueba)

        --------------------------------------------------------
        boss:
            - dispara rayos (no es prioridad)
            - sin saltos
            - buscar imagen
            - puede golpear de forma directa
            - cambie de direccion segun la posicion del personaje
        --------------------------------------------------------
            - agregar musica (no es prioridad)
         */
    }
}
