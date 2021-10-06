using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour
{
    private float speed = 7f;
    private float minRangeToMove = 10f;
    public bool Running { get; set; }
    private CapsuleCollider2D collider;

    private void Awake()
    {
        Running = false;
        collider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        Vector3 heroPos = GameManager.Instance.hero.transform.position;
        Vector3 demonPos = transform.position;
        if (Vector3.Distance(demonPos, heroPos) <= minRangeToMove)
        {
            this.Running = true;
        }
        if (this.Running)
        {
            transform.position += Vector3.MoveTowards(demonPos, heroPos, speed * Time.deltaTime);
        }
    }
}
