using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemonController : MonoBehaviour
{
    private float speed = 3f;
    private float minRangeToMove = 50f;
    private float minDistanceFromFloor = 6.81f;
    private float minRangeToAttack = 10f;

    private Animator animator;
    private GameObject fireBreath;
    private Transform firePoint;
    private Slider slider;
    private float timeBetweenAttacks = 5f;
    private float lastAttackTime = 0f;
    public bool Running { get; set; }

    private void Awake()
    {
        Running = false;
        firePoint = transform.Find("FirePoint");
        slider = transform.Find("EnemyHealth").Find("HealthBar").GetComponent<Slider>();
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 heroPos = GameManager.Instance.hero.transform.position;
        Vector3 demonPos = transform.position;
        float distance = Vector3.Distance(demonPos, heroPos);
        float gameTime = Time.time;
        if (distance <= minRangeToMove && distance > minRangeToAttack)
        {
            this.Running = true;
        }
        if (demonPos.x >= heroPos.x)
        {
            transform.localScale = new Vector3(
                1f,
                1f,
                1f
            );
        }
        else
        {
            transform.localScale = new Vector3(
                -1f,
                1f,
                1f
            );
        }
        if (this.Running)
        {
            transform.position = Vector3.MoveTowards(transform.position, heroPos, speed * Time.deltaTime);
        }
        if (distance <= minRangeToAttack && fireBreath == null && gameTime - lastAttackTime > timeBetweenAttacks)
        {
            Fire();
            lastAttackTime = gameTime;
        }
        if (transform.position.y <= minDistanceFromFloor)
        {
            transform.position = new Vector3(
                transform.position.x,
                minDistanceFromFloor,
                0f
            );
        }
    }

    private void Fire()
    {
        animator.SetTrigger("Fire");
        if (transform.localScale.x == -1)
        {
            fireBreath = Instantiate(GameManager.Instance.breath, firePoint.position, Quaternion.identity);
            fireBreath.transform.localScale = new Vector3(
                -1f,
                1f,
                1f
            );
            fireBreath.transform.parent = transform;
        }
        else
        {
            fireBreath = Instantiate(GameManager.Instance.breath, firePoint.position, Quaternion.identity);
            fireBreath.transform.parent = transform;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Bullet"))
        {
            int damage = collision.transform.GetComponent<BulletController>().damage;
            slider.value -= damage;

            if (slider.value <= 0)
            {
                // Debe morir
                animator.SetTrigger("IsDead");
                Running = false;
            }
        }
    }
}
