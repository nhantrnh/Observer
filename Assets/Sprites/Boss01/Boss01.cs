using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss01 : MonoBehaviour
{
    private Animator animator;
    private int health = 10;
    public float attackRange = 5f; // Khoảng cách tấn công
    public float attackInterval = 1f; // Thời gian giữa các lần tấn công
    private float attackTimer;
    private bool isPlayerInRange = false; // Kiểm tra xem nhân vật chính có trong phạm vi hay không
    private int attackCount = 0; // Biến đếm số lần tấn công

    void Start()
    {
        animator = GetComponent<Animator>();
        attackTimer = attackInterval; // Khởi tạo timer với khoảng thời gian giữa các lần tấn công
        animator.SetTrigger("Idle"); // Đảm bảo bắt đầu ở trạng thái Idle
    }

    void Update()
    {
        if (isPlayerInRange && attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        if (!isPlayerInRange && attackTimer > 0f)
        {
            animator.SetTrigger("Idle");
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        MainController mainController = collider.GetComponent<MainController>();
        if (mainController != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, mainController.transform.position);

            if (distanceToPlayer <= attackRange)
            {
                isPlayerInRange = true;

                if (attackTimer <= 0f)
                {
                    PerformAction(mainController);
                    attackTimer = attackInterval; // Reset timer sau mỗi lần tấn công
                }
            }
            else
            {
                isPlayerInRange = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponent<MainController>())
        {
            isPlayerInRange = false;
            animator.SetTrigger("Idle");
            animator.SetTrigger("Spell");
            animator.SetTrigger("Attack");
        }
    }

    void PerformAction(MainController mainController)
    {
        if (attackCount < 2)
        {
            Attack(mainController);
            attackCount++;
        }
        else
        {
            Spell(mainController);
            attackCount = 0; // Reset đếm sau khi sử dụng Spell
        }
    }

    void Attack(MainController mainController)
    {
        animator.SetTrigger("Attack");
        // Đợi animation attack hoàn thành và sau đó trừ máu
        StartCoroutine(WaitForAttackAnimation(mainController));
    }

    void Spell(MainController mainController)
    {
        animator.SetTrigger("Spell");
        // Đợi animation spell hoàn thành và sau đó trừ máu
        StartCoroutine(WaitForSpellAnimation(mainController));
    }

    // Đợi animation attack hoàn thành rồi mới trừ máu
    private IEnumerator WaitForAttackAnimation(MainController mainController)
    {
        // Kiểm tra thời gian animation
        yield return new WaitForSeconds(0.5f);  // Giả sử animation Attack dài 0.5s
        if (mainController.HealthPoint > 0)
        {
            mainController.ChangeHealthPoint(-1);  // Trừ máu cho người chơi
        }
    }

    // Đợi animation spell hoàn thành rồi mới trừ máu
    private IEnumerator WaitForSpellAnimation(MainController mainController)
    {
        // Kiểm tra thời gian animation
        yield return new WaitForSeconds(0.7f);  // Giả sử animation Spell dài 0.7s
        if (mainController.HealthPoint > 0)
        {
            mainController.ChangeHealthPoint(-2);  // Trừ máu cho người chơi nhiều hơn cho spell
        }
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
        Destroy(gameObject, 2f);
    }
}
