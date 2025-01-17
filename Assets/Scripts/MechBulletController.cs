using UnityEngine;

public class MechBulletController : MonoBehaviour
{
    public float speed = 30f; // Tốc độ viên đạn
    public int damage = -1; // Sát thương
    private Vector3 direction; // Hướng di chuyển của viên đạn

    void Start()
    {
        // Đặt trọng lực cho viên đạn là 0
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        // Tạo hướng ngẫu nhiên
        // Tạo hướng ngẫu nhiên trong hai khoảng góc
        float angle;
        if (Random.value > 0.5f)
        {
            // Chọn khoảng [-45, 45]
            angle = Random.Range(-45f, 45f);
        }
        else
        {
            // Chọn khoảng [135, -135] (tương đương với [-135, -45] do vòng tròn 360 độ)
            angle = Random.Range(135f, 360f); // Chọn từ 135 đến 360
            angle = angle > 180 ? angle - 360 : angle; // Chuyển đổi về khoảng [-135, -180]
        }

        direction = Quaternion.Euler(0, 0, angle) * Vector3.right; // Tính toán hướng từ góc
        direction.Normalize();
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