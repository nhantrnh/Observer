using UnityEngine;

public class ChaosBossController : MonoBehaviour
{
    public float idleDuration = 3f;
    
    private bool isAttack = false;

    private float idleTimer;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttack) {
            idleTimer -= Time.deltaTime;
            if (idleTimer < 0) {
                isAttack = true;
                animator.SetBool("isAttack", true);
                
            }
        }
    }
}
