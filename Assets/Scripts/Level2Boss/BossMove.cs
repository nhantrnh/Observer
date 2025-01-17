using UnityEngine;
public class DeathMove : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange = 3f;
    private float summonTimer = 0f; // Thêm biến đếm thời gian
    Transform player;
    Rigidbody2D rb;
    Boss boss;

    public GameObject summonPrefab;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        summonTimer = 0f; // Reset timer khi state bắt đầu
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        // Cập nhật timer và kiểm tra điều kiện
        summonTimer += Time.deltaTime;
        if (summonTimer >= 3f)
        {
            animator.SetTrigger("Summon");
            Vector2 spawnDirection = rb.transform.right;
            Instantiate(summonPrefab, rb.position + Vector2.up * 3f + (Vector2)spawnDirection * 2f, Quaternion.identity);
            summonTimer = 0f; // Reset timer sau khi trigger
        }
        else if (Vector2.Distance(player.position, rb.position) <= attackRange)
        {
            animator.SetTrigger("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Summon"); // Thêm reset cho trigger Summon
    }
}