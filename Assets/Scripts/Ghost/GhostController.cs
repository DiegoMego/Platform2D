using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    private float speed = 7f;
    public bool Running { get; set; }

    private void Awake()
    {
        Running = false;
    }

    private void Update()
    {
        if (Running)
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }
    }
}
