using UnityEngine;

public class MechBulletController : MonoBehaviour
{
    public float speed = 20f; // Tốc độ viên đạn
    public int damage = -1; // Sát thương
    private Vector3 direction; // Hướng di chuyển của viên đạn

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
    }


    void Start()
    {
        // Đặt trọng lực cho viên đạn là 0
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void Update()
    {
        // Di chuyển viên đạn theo hướng ngẫu nhiên
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MainController mainController = collision.GetComponent<MainController>();
        if (mainController != null)
        {
            if (mainController.HealthPoint > 0)
            {
                mainController.ChangeHealthPoint(-1);
                Destroy(gameObject);
            }

        }
    }
}