using UnityEngine;
using UnityEngine.UI;

public class BulletController : MonoBehaviour
{
    public int damage = 50;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            //Aumentar la barra de poder
            if (GameManager.Instance.powerBarSlider.value < GameManager.Instance.powerBarSlider.maxValue) GameManager.Instance.powerBarSlider.value += GameManager.Instance.powerBarSlider.maxValue * 0.25f;

            //Si la bala choca con un objeto, se debe destruir la bala.
            Destroy(transform.gameObject);
        }
    }
}
