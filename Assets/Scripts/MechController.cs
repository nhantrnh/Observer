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
        Vector3 bulletStartPosition = new Vector3(transform.position.x, transform.position.y + bulletHeightOffset, transform.position.z);
        GameObject bullet = Instantiate(bulletPrefab, bulletStartPosition, Quaternion.identity);
        bullet.transform.localScale = new Vector3(movingRight ? 1 : -1, 1, 1); // Đảo chiều nếu cần
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
    }

    private System.Collections.IEnumerator WaitAndDestroy()
    {
        // Chờ cho đến khi animation "Death" hoàn thành
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject); // Xóa Mech
    }
}