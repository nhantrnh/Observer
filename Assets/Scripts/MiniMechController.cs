using UnityEngine;

public class MiniMechController : MonoBehaviour
{
    public float speed = 2f;
    public float waitTime = 2f;
    public float leftBoundary = -10f;
    public float rightBoundary = 10f;
    private bool movingRight = true;
    private Animator animator;
    private float waitTimer = 0f;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (waitTimer <= 0)
        {
            Move();
        }
        else
        {
            animator.Play("Idle");
            waitTimer -= Time.deltaTime;
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
        animator.Play("Idle");
        waitTimer = waitTime;
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        MainController mainController = collider.GetComponent<MainController>();
        if (mainController != null)
        {
            Rigidbody2D rb = mainController.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Kiểm tra xem MainController có đang rơi xuống không
                if (rb.linearVelocity.y < 0) // Nếu tốc độ y âm, tức là đang rơi
                {
                    Destroy(gameObject); // Xóa MiniMech
                }
                else if (mainController.HealthPoint > 0)
                {
                    mainController.ChangeHealthPoint(-1);
                }
            }
        }
    }
}
