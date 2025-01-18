using System;
using System.Collections;
using UnityEngine;

public class Boss01 : MonoBehaviour
{
    private Animator animator;
    public int health = 20;
    public bool isInvulnerable = false;
    public int attackDamage = 1;

    public Vector3 attackOffset;
    public float attackRange = 2f;
    public LayerMask attackMask;

    public Transform player;
    public bool isFlipped = true;
    public GameObject Portal2;
    public float speed = 2.5f;
    
    private Rigidbody2D rb;
    private bool isAttacking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        LookAtPlayer();
    }

    void Update()
    {
        if (health <= 0)
        {
            animator.SetTrigger("Death");
            Destroy(gameObject, 1f);
            Portal2.SetActive(true);
        }

        // Gọi Coroutine để tấn công sau 3 giây
        if (!isAttacking)
        {
            StartCoroutine(AttackAfterDelay(3f));  // Tấn công sau 3 giây
        }
    }

    // Đảm bảo boss luôn quay về phía player
    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.x *= 1f;

        if (transform.position.x > player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            isFlipped = true;
        }
        else if (transform.position.x < player.position.x && isFlipped)
        {
            transform.localScale = -flipped;
            isFlipped = false;
        }

        animator.SetTrigger("Walk");
    }

    // Coroutine trì hoãn việc tấn công
    private IEnumerator AttackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Gọi hàm Attack khi đủ thời gian
        Attack();
    }

    // Hàm tấn công
public void Attack()
{
    isAttacking = true;  // Đánh dấu là đang tấn công

    // Tính toán vị trí tấn công dựa vào vị trí của Boss và offset
    Vector3 pos = transform.position;
    pos += transform.right * attackOffset.x;
    pos += transform.up * attackOffset.y;

        int randomAttack = UnityEngine.Random.Range(0, 2);

        if (randomAttack == 0)
        {
            animator.SetTrigger("Attack");
        }
        else
        {
            animator.SetTrigger("Spell");
        }

        resetIdle();
        float distanceToPlayer = Vector2.Distance(player.position, transform.position);

    if (distanceToPlayer <= attackRange)
    {
         player.GetComponent<MainController>().ChangeHealthPoint(-attackDamage);
         
    }

    }


    public void Destroy()
    {
        animator.SetTrigger("Death");
        Destroy(gameObject, 1f);
        Portal2.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        if (isInvulnerable) return;

        health -= damage;
        if (health <= 0)
        {
            animator.SetTrigger("Death");
            Destroy(gameObject, 1f);
            Portal2.SetActive(true);
        }
        else
        {
            animator.SetTrigger("Hurt");
            resetIdle();
            StartCoroutine(AttackAfterDelay(3f));  // Reset sau 1 giây
        }
    }

    public void resetIdle()
    {
        animator.SetTrigger("Idle");
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Vector2 size = new Vector2(attackRange * 2, attackRange);
        Gizmos.DrawWireCube(pos, size);
    }
}
