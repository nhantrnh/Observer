using UnityEngine;
using System;

public class MechController : MonoBehaviour
{
    public float speed = 2f;
    public float waitTime = 2f;
    public float leftBoundary = -10f;
    public float rightBoundary = 10f;
    public GameObject bulletPrefab; // Tham chiếu đến prefab viên đạn
    public GameObject targetObject; // Tham chiếu đến GameObject mà bạn muốn thay đổi collider

    private bool movingRight = true;
    private Animator animator;
    private float waitTimer = 0f;
    private float attackTimer = 5f; // Thời gian chờ để bắn

    public int healthPoint = 70;
    public int currentHp;
    private bool isAlive = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHp = 70;
    }

    void Update()
    {
        if (!isAlive)
        {
            return;
        }

        if (waitTimer <= 0)
        {
            Move();
        }
        else
        {
            animator.Play("Idle");
            waitTimer -= Time.deltaTime;
        }

        // Giảm timer và bắn đạn khi đến thời gian
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            Shoot();
            attackTimer = 5f; // Reset timer
        }

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Move()
    {
        animator.Play("Walk");

        Vector3 direction = movingRight ? Vector3.right : Vector3.left;
        transform.Translate(direction * speed * Time.deltaTime);

        if ((movingRight && transform.position.x > rightBoundary) || (!movingRight && transform.position.x < leftBoundary))
        {
            movingRight = !movingRight;
            Flip();
            Stop();
        }
    }

    void Stop()
    {
        if (!isAlive)
        {
            animator.Play("Death");
            return;
        }

        animator.Play("Idle");
        waitTimer = waitTime;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public float bulletHeightOffset = 40f; // Độ cao của viên đạn so với Mech

    void Shoot()
    {
        // Sinh ngẫu nhiên góc cho viên đạn trung tâm trong khoảng (20, 160)
        float centralAngle = UnityEngine.Random.Range(20f, 160f);

        // Tính các góc cho 5 viên đạn xung quanh viên đạn trung tâm, với khoảng cách đều
        float spread = 20f; // Góc giữa các viên đạn
        float[] angles = {
            centralAngle - 2 * spread,
            centralAngle - spread,
            centralAngle,
            centralAngle + spread,
            centralAngle + 2 * spread
        };

        foreach (float angle in angles)
        {
            // Tính toán hướng dựa trên góc
            Vector3 direction = Quaternion.Euler(0, 0, movingRight ? angle : -angle) * Vector3.right;

            // Tạo vị trí bắt đầu cho viên đạn
            Vector3 bulletStartPosition = new Vector3(transform.position.x, transform.position.y + bulletHeightOffset, transform.position.z);

            // Tạo viên đạn và thiết lập hướng
            GameObject bullet = Instantiate(bulletPrefab, bulletStartPosition, Quaternion.identity);
            MechBulletController bulletController = bullet.GetComponent<MechBulletController>();
            if (bulletController != null)
            {
                bulletController.SetDirection(direction); // Truyền hướng cho viên đạn
            }

            // Đảo chiều sprite của viên đạn nếu cần
            bullet.transform.localScale = new Vector3(movingRight ? 1 : -1, 1, 1);
        }
    }


    private void OnTriggerStay2D(Collider2D collider)
    {
        MainController mainController = collider.GetComponent<MainController>();
        if (mainController != null)
        {
            if (mainController.HealthPoint > 0)
            {
                mainController.ChangeHealthPoint(-1);
            }
        }
    }

    public void ChangeHealthPoint(int value)
    {
        if (currentHp <= 0) return;

        // Calculate
        currentHp = Math.Clamp(currentHp + value, 0, healthPoint);
    }

    void Die()
    {
        isAlive = false; // Đánh dấu là chết
        Stop(); // Dừng di chuyển
        StartCoroutine(WaitAndDestroy()); // Gọi coroutine để chờ và xóa

        // Thay đổi collider của targetObject thành trigger
        if (targetObject != null)
        {
            Collider2D targetCollider = targetObject.GetComponent<Collider2D>();
            if (targetCollider != null)
            {
                targetCollider.isTrigger = true; // Biến collider thành trigger
            }
        }

        // Giảm opacity của vật thể
        SpriteRenderer spriteRenderer = targetObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color32(255, 255, 255, 128);
        }
    }

    private System.Collections.IEnumerator WaitAndDestroy()
    {
        // Chờ cho đến khi animation "Death" hoàn thành
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject); // Xóa Mech
    }
}