using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostController : MonoBehaviour
{
    private float speed = 4f;
    public bool Running { get; set; }
    private Slider slider;
    private Animator animator;
    private float minAggroDistance = 10f;

    private void Awake()
    {
        Running = false;
        animator = GetComponent<Animator>();
        slider = transform.Find("EnemyHealth").Find("HealthBar").GetComponent<Slider>();
    }

    private void Update()
    {
        Vector3 ghostPos = transform.position;
        Vector3 heroPos = GameManager.Instance.hero.transform.position;

        float distance = Vector3.Distance(ghostPos, heroPos);

        if (distance <= minAggroDistance)
        {
            this.Running = true;
        }

        if (Running)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
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
