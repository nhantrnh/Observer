using System;
using UnityEngine;

public class Boss01 : MonoBehaviour
{
    private Animator animator;
    private int health = 10;
    public float attackRange = 20f; // Khoảng cách tấn công
    public float attackInterval = 3f; // Thời gian giữa các lần tấn công
    private float attackTimer;
    private bool isPlayerInRange = false; // Kiểm tra xem nhân vật chính có trong phạm vi hay không

    void Start()
    {
        animator = GetComponent<Animator>();
        attackTimer = attackInterval; // Khởi tạo timer với khoảng thời gian giữa các lần tấn công
        animator.SetTrigger("Idle"); // Đảm bảo bắt đầu ở trạng thái Idle
    }

    void Update()
    {
        // Chỉ giảm attackTimer khi nhân vật chính đang trong phạm vi tấn công
        if (isPlayerInRange && attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
            animator.SetTrigger("Idle");

        }

        // Nếu không có gì xảy ra (không tấn công và không bị tấn công), quay lại trạng thái Idle
        if (!isPlayerInRange && attackTimer > 0f)
        {
            animator.SetTrigger("Idle"); // Trở về trạng thái Idle nếu không có gì xảy ra
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        MainController mainController = collider.GetComponent<MainController>();
        if (mainController != null)
        {
            // Kiểm tra khoảng cách giữa Boss và nhân vật chính
            float distanceToPlayer = Vector3.Distance(transform.position, mainController.transform.position);

            if (distanceToPlayer <= attackRange)
            {
                isPlayerInRange = true;

                // Nếu tấn công và timer đã hết
                if (attackTimer <= 0f)
                {
                    Attack(mainController);
                    attackTimer = attackInterval; // Reset timer sau mỗi lần tấn công
                    animator.SetTrigger("Idle");
                }
            }
            else
            {
                isPlayerInRange = false; // Nếu không trong phạm vi, trở lại trạng thái Idle
                animator.SetTrigger("Idle");    
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // Khi nhân vật chính ra khỏi phạm vi tấn công, dừng tấn công
        if (collider.GetComponent<MainController>())
        {
            isPlayerInRange = false;
            animator.SetTrigger("Idle"); // Trở về trạng thái Idle khi không còn trong phạm vi
        }
    }

    void Attack(MainController mainController)
    {
        animator.SetTrigger("Attack");
        // Giảm máu của nhân vật chính nếu họ còn sống
        if (mainController.HealthPoint > 0)
        {
            mainController.ChangeHealthPoint(-1);
        }
    }

    public void FixedUpdate()
    {
        TakeDamage(1);
    }

    public void Fix()
    {
        Die();
        Destroy(gameObject);
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health > 0)
        {
            animator.SetTrigger("Hurt");
        }
        else
        {
            Die();
        }
    }

    void Die()
    {
        animator.SetTrigger("Death");
        // Logic khi chết, như vô hiệu hóa collider, ngừng di chuyển, v.v.
    }
}
