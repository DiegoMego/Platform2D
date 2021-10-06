using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour
{
    private float speed = 3f;
    private float minRangeToMove = 10f;
    private Vector3 direcion = Vector3.left;
    public bool Running { get; set; }

    private void Awake()
    {
        Running = false;
    }

    private void Update()
    {
        Vector3 heroPos = GameManager.Instance.hero.transform.position;
        Vector3 demonPos = transform.position;
        Debug.Log($"Distancia: {Vector3.Distance(demonPos, heroPos)}");
        if (Vector3.Distance(demonPos, heroPos) <= minRangeToMove)
        {
            this.Running = true;
        }
        if (this.Running)
        {
            if (demonPos.x >= heroPos.x)
            {
                direcion = Vector3.left;
                transform.localScale = new Vector3(
                    1f,
                    1f,
                    1f
                );
            }
            else
            {
                direcion = Vector3.right;
                transform.localScale = new Vector3(
                    -1f,
                    1f,
                    1f
                );
            }
            //transform.position += direcion * speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, heroPos, speed * Time.deltaTime);
        }
    }
}
