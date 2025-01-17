using System.Dynamic;
using UnityEngine;

public class SummonMove : StateMachineBehaviour
{
    public float speed = 7.5f;
    private float summonTimer = 0f;
    Transform player;
    Rigidbody2D rb;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        summonTimer = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, player.position, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        summonTimer += Time.deltaTime;
        if (summonTimer >= 2f)
        {
            animator.SetTrigger("Destroy");
        }

        if (Vector2.Distance(player.position, rb.position) <= 1f)
        {
            animator.SetTrigger("Destroy");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Destroy");
    }

}
