using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed = 8f;
    private float timeTilDestroyed = 15f;
    private float time = 0f;
    private Vector3 direction = Vector3.right;

    private void Start()
    {
        if (transform.rotation.eulerAngles.z == 180f)
        {
            direction = Vector3.left;
        }
    }

    private void Update()
    {
        if (time > timeTilDestroyed)
        {
            Destroy(transform.gameObject);
        }
        time += Time.deltaTime;
        transform.position += direction * speed * Time.deltaTime;
    }
}
