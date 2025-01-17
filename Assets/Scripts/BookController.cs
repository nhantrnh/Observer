using UnityEngine;
using UnityEngine.EventSystems;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public GameObject mainChar;
    private Transform mcTransform;
    private MainController mcController;
    private Vector3 offset;
    private Animator animator;
    public float distance;
    private float followSpeed;

    public bool isFlipped = true;

    public GameObject projectilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mcTransform = mainChar.GetComponent<Transform>();
        mcController = mainChar.GetComponent<MainController>();
        animator = GetComponent<Animator>();
        followSpeed =mcController.speed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (mainChar != null)
        {
            if (mcController.isLookingLeft) {
                offset = new Vector3(-distance, distance,0);
                if (!isFlipped)
                {
                    transform.localScale = flipped;
                    transform.Rotate(0f, 180f, 0f);
                    isFlipped = true;
                }            
            } else {
                offset = new Vector3( distance, distance, 0);
                if (isFlipped)
                {
                    transform.localScale = flipped;
                    transform.Rotate(0f, 180f, 0f);
                    isFlipped = false;
                }
            }
            Vector3 targetPosition = mcTransform.position + offset;
            Vector3 move = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            transform.position = move;
            animator.SetFloat("Speed", mcController.regconizedSpeed);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Launch();
        }
    }

    void Launch()
    {
        Vector3 direction = isFlipped ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0);
        GameObject projectileObject = Instantiate(projectilePrefab, transform.position + direction * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        
        projectile.Launch(direction, 500);
    }
}
