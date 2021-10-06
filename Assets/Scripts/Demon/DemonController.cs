using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour
{
    private float speed = 3f;
    private float minRangeToMove = 10f;
    private float minDistanceFromFloor = 5.6f;
    public bool Running { get; set; }

    private void Awake()
    {
        Running = false;
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
            transform.position = Vector3.MoveTowards(transform.position, heroPos, speed * Time.deltaTime);
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
}
